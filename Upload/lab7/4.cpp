#include <glm/glm.hpp>
#include <GLFW/glfw3.h>
#include <GL/gl.h>
#include <GL/glu.h>
#include <iostream>
#include <vector>
#include <math.h>

using namespace std;
using namespace glm;

#define PI   	 3.14159265359
#define TWOPI    PI*2

GLFWwindow *window;
int w, h;
double mouseX, mouseY;

vector<double> knots;   	 
vector<dvec2> control;   	 
float cRadius = 0.013f;   	 
int selected = -1;   		 
bool movePoint = false;    
bool showPoints = true;   	 
bool showColours = false;    
bool niceLines = true;    
bool sRevolution = false;    

int k = 3;   			 
double uinc = 0.001;   	 
double vinc = PI/16;

bool rotating = false;   	 
float yangle = 0.f;   		 
float zangle = 0.f;   		 
float rotSpeed = 100.f;    
float scale = 1.f;    
float zoomSpeed = 0.02f;

int delta(double u) {
    int m = control.size();
    for (int i = 0; i <= m + k - 1; i++) {
   	 if (u >= knots[i] && u < knots[i + 1])
   		 return i;
    }
    return -1;
}

dvec2 bspline(double u, int d) {

    dvec2 *c = new dvec2[control.size()];
    for (int i = 0; i <= k - 1; ++i) {
   	 c[i] = control[d - i];
    }

    for (int r = k; r >= 2; --r) {
   	 int i = d;
   	 for (int s = 0; s <= r - 2; ++s) {
   		 double u_i = knots[i];
   		 double u_ir1 = knots[i + r - 1];
   		 double omega = (u - u_i) / (u_ir1 - u_i);
   		 c[s] = omega * c[s] + (1.0 - omega) * c[s + 1];
   		 i--;
   	 }
    }

    dvec2 result = c[0];
    delete[] c;
    return result;
}

void generateKnots() {
    knots.clear();

    for (int i = 0; i < k; i++)
   	 knots.push_back(0.0);

    int middle = control.size() - k;
    for (int i = 0; i < middle; i++)
   	 knots.push_back(double(i+1) / (middle+1));

    for (int i = 0; i < k; i++)
   	 knots.push_back(1);
}

void render() {
    glEnable(GL_DEPTH_TEST);
    glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
    if (niceLines && !sRevolution) {
   	 glEnable(GL_LINE_SMOOTH);
   	 glHint(GL_LINE_SMOOTH_HINT, GL_NICEST);
   	 glEnable(GL_BLEND);
   	 glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
    }
    else {
   	 glDisable(GL_LINE_SMOOTH);
   	 glDisable(GL_BLEND);
    }
    glMatrixMode(GL_MODELVIEW);
    glLoadIdentity();
    glScalef(scale, scale, scale);   		 
    glRotatef(yangle, 0.0f, 1.0f, 0.0f);
    glRotatef(zangle, 0.0f, 0.0f, 1.0f);
    glMatrixMode(GL_PROJECTION);
    glLoadIdentity();
    glOrtho(-1, 1, -1, 1, -10, 10);
    if (showPoints) {
   	 glBegin(GL_QUADS);
   	 for (int i = 0; i < control.size(); i++) {
   		 float pr = 245.f;
   		 float pg = 40.f;
   		 float pb = 145.f;
   		 if (selected == i) {
   			 pg = 0.f;
   			 pb = 0.f;
   		 }
   		 glColor3f(pr, pg, pb);
   		 glVertex3f(control[i].x + cRadius, control[i].y + cRadius, 0);
   		 glColor3f(pr, pg, pb);
   		 glVertex3f(control[i].x + cRadius, control[i].y - cRadius, 0);
   		 glColor3f(pr, pg, pb);
   		 glVertex3f(control[i].x - cRadius, control[i].y - cRadius, 0);
   		 glColor3f(pr, pg, pb);
   		 glVertex3f(control[i].x - cRadius, control[i].y + cRadius, 0);
   	 }

   	 glEnd();
    }
    glLineWidth(3.5);
    glBegin(GL_LINE_STRIP);
    generateKnots();
    for (double u = knots[k-1] + uinc; u <= knots[control.size()]; u += uinc) {
   	 int d = delta(u);
   	 if (control.size() >= d) {
   		 float cr = 1;
   		 float cg = 1;
   		 float cb = 1;
   		 if (showColours) {
   			 cr = 0.5 * (sin(101 * u) + 1);
   			 cg = 0.5 + 0.25 * (cos(11 * u) + 1);
   			 cb = 0.5 * (sin(71 * u) + 1);
   		 }
   		 glColor3f(cr, cg, cb);
   		 dvec2 point = bspline(u, d);
   		 glVertex3f(point.x, point.y, 0);
   	 }
    }
    glEnd();
}

void keyboard (GLFWwindow *sender, int key, int scancode, int action, int mods) {
    if (key == GLFW_KEY_ESCAPE && action == GLFW_PRESS)
   	 glfwSetWindowShouldClose(window, GL_TRUE);
    if (key == GLFW_KEY_UP && (action == GLFW_PRESS || action == GLFW_REPEAT))
   	 uinc *= 0.9;

    if (key == GLFW_KEY_DOWN && (action == GLFW_PRESS || action == GLFW_REPEAT))
   	 uinc *= 1.1;
    if (key == GLFW_KEY_HOME && action == GLFW_PRESS)
   	 showPoints = !showPoints;

    if (key == GLFW_KEY_END && action == GLFW_PRESS)
   	 niceLines = !niceLines;
}

void mouseClick (GLFWwindow *sender, int button, int action, int mods) {

	if (action == GLFW_PRESS) {
   	 selected = -1;
   	 double x = (2 * mouseX / w) - 1;
   	 double y = (-2 * mouseY / h) + 1;
   	 for (int i = 0; i < control.size(); i++) {
   		 if (abs(control[i].x - x) <= cRadius && abs(control[i].y - y) <= cRadius) {
   			 selected = i;
   			 movePoint = true;
   		 }
   	 }

   	 if (button == GLFW_MOUSE_BUTTON_LEFT && selected == -1) {
   		 control.push_back(vec2(x, y));
   	 }

   	 else if (button == GLFW_MOUSE_BUTTON_RIGHT && selected >= 0) {
   		 control.erase(control.begin() + selected);
  
   		 selected = -1;
   	 }

   	 else if (button == GLFW_MOUSE_BUTTON_MIDDLE) {
   		 glfwGetCursorPos(window, &mouseX, &mouseY);
   		 rotating = true;
   	 }
	}

    if (action == GLFW_RELEASE) {
   	 movePoint = false;
   	 rotating = false;
    }
}

void mousePos(GLFWwindow *sender, double x, double y) {
    if (rotating) {
   	 yangle += rotSpeed * float(x - mouseX) / float(w);
   	 zangle += rotSpeed * float(y - mouseY) / float(h);
    }

    mouseX = x;
    mouseY = y;

    if (movePoint && selected >= 0) {
   	 double newx = (2 * mouseX / w) - 1;
   	 double newy = (-2 * mouseY / h) + 1;
   	 control[selected] = vec2(newx, newy);
    }
}


void mouseScroll(GLFWwindow *sender, double x, double y)
{
    scale += zoomSpeed * y;
}


int main () {
    if (!glfwInit())  {
   	 return 1;
    }
    window = glfwCreateWindow (800, 800, "Степанов Д.М. 307Б вариант 10, key up-увеличиваем точность, down-уменьшаем, home-скрываем контрольные точки", NULL, NULL);
    if (!window) return 1;
    glfwMakeContextCurrent (window);
    glfwSetKeyCallback (window, keyboard);
    glfwSetMouseButtonCallback (window, mouseClick);
    glfwSetCursorPosCallback (window, mousePos);
    glfwSetScrollCallback(window, mouseScroll);
    while (!glfwWindowShouldClose (window)) {
   	 glfwGetFramebufferSize (window, &w, &h);
   	 glViewport (0, 0, w, h);

   	 render ();

   	 glfwSwapBuffers (window);
   	 glfwPollEvents();
    }
    glfwDestroyWindow (window);
    glfwTerminate();
    return 0;
}
