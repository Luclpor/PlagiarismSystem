#include <iostream>
#include <cmath>
#include <tinyxml2.h>
#define GLEW_STATIC
#include <GL/glew.h>
#include <GLFW/glfw3.h>
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>
#include "Shader.h"
#include "Camera.h"
using namespace std;

struct Point
{
	Point(double x, double y, double z): x(x), y(y), z(z) {}
	double x = 0;
	double y = 0;
	double z = 0;
	double h = 0;
	Point(double x, double y, double z, double h): x(x), y(y), z(z), h(h) {}
};

class Cube
{
public:
	std::vector<Point> points = {
    	Point(-0.5f, -0.5f, -0.5f),
    	Point(0.5f, -0.5f, -0.5f),
    	Point(0.5f, -0.5f, 0.5f),
    	Point(-0.5f, -0.5f, 0.5f),
    	Point(-0.5f, 0.5f, -0.5f),
    	Point(0.5f, 0.5f, -0.5f),
    	Point(0.5f, 0.5f, 0.5f),
    	Point(-0.5f, 0.5f, 0.5f)
	};

};

void key_callback(GLFWwindow* window, int key, int scancode, int action, int mode);
void mouse_callback(GLFWwindow* window, double xpos, double ypos);
void scroll_callback(GLFWwindow* window, double xoffset, double yoffset);
void do_movement();
void new_func_size_callback(GLFWwindow* window, int width, int heigh);
void save_state();
void load_state();

GLfloat* get_figure();

const GLuint WIDTH = 800, HEIGHT = 600;

glm::vec3 cameraFront = glm::vec3(0.0f, 0.0f, -1.0f);


Camera  camera(glm::vec3(0.0f, 0.0f, 3.0f));
GLfloat lastX  =  WIDTH  / 2.0;
GLfloat lastY  =  HEIGHT / 2.0;
bool keys[1024];
bool ret = true;
bool load = false;
bool change = false;

GLFWwindow* window = nullptr;

GLfloat shiness = 32.0f;
GLfloat APPROX = 0.01f;
GLfloat chill_height = 1.25f;

glm::vec3 lightPos(1.2f, 1.0f, 2.0f);
glm::vec3 LightSourceColour(1.0f, 1.0f, 1.0f);
GLfloat deltaTime = 0.0f;
GLfloat lastFrame = 0.0f;


GLfloat x_rotation = 0.0f;
GLfloat y_rotation = 0.0f;
GLfloat z_rotation = 0.0f;

GLfloat x_rotation_save = 0.0f;
GLfloat y_rotation_save = 0.0f;
GLfloat z_rotation_save = 0.0f;

GLfloat x_rotation_load = 0.0f;
GLfloat y_rotation_load = 0.0f;
GLfloat z_rotation_load = 0.0f;

GLfloat scale = 1.0f;

void new_func_size_callback(GLFWwindow* window, int width, int height){
	if (!height) {
    	height = 0;
	}
	glViewport(0, 0, width, height);
}

GLfloat* get_figure(){
	GLfloat* vertex =(GLfloat*) malloc(sizeof(GLfloat) * (1 + (unsigned)(2.0f/APPROX)) * 9 * 4 * 2);
	GLint j = 0;
	Cube cube;
	for(GLfloat i = 0.0f; i <= 2.0f*M_PI; i += M_PI*APPROX){
    	vertex[j++] = 0.0f;
    	vertex[j++] = -chill_height / 2.0f;
    	vertex[j++] = 0.0f;
    	vertex[j++] = 0.0f;
    	vertex[j++] = -1.0f;
    	vertex[j++] = 0.0f;
    	vertex[j++] = 0.6f * cos(i);
    	vertex[j++] = -chill_height / 2.0f;
    	vertex[j++] = 0.6f * sin(i);
    	vertex[j++] = 0.0f;
    	vertex[j++] = -1.0f;
    	vertex[j++] = 0.0f;
    	vertex[j++] = 0.6f * cos(i + M_PI*APPROX);
    	vertex[j++] = -chill_height / 2.0f;
    	vertex[j++] = 0.6f * sin(i + M_PI*APPROX);
    	vertex[j++] = 0.0f;
    	vertex[j++] = -chill_height / 2.0f;
    	vertex[j++] = 0.0f;
	}
	for(GLfloat i = 0.0f; i <= 2*M_PI; i += M_PI*APPROX){
    	vertex[j++] = 0.0f;
    	vertex[j++] = chill_height / 2;
    	vertex[j++] = 0.0f;
    	vertex[j++] = -1.0f;
    	vertex[j++] = 1.0f;
    	vertex[j++] = 0.0f;
    	vertex[j++] = 0.6f * cos(i);
    	vertex[j++] = chill_height / 2;
    	vertex[j++] = 0.6f * sin(i);
    	vertex[j++] = -1.0f;
    	vertex[j++] = 1.0f;
    	vertex[j++] = 0.0f;
    	vertex[j++] = 0.6f * cos(i + M_PI*APPROX);
    	vertex[j++] = chill_height / 2;
    	vertex[j++] = 0.6f * sin(i + M_PI*APPROX);  
    	vertex[j++] = -1.0f;
    	vertex[j++] = 1.0f;
    	vertex[j++] = 0.0f;
	}
	for(GLfloat i = 0.0f; i <= 2*M_PI; i += M_PI*APPROX){
    	GLfloat ax = cos(i) - cos(i + M_PI*APPROX);
    	GLfloat ay = 0.0f;
    	GLfloat az = sin(i) - sin(i + M_PI * APPROX);
    	GLfloat bx = 0.0f;
    	GLfloat by = 1.0f;
    	GLfloat bz = 0.0f;
    	GLfloat nx = ay * bz - az * by;
    	GLfloat ny = 0.0f;
    	GLfloat nz = ax*by - ay*bx;
    	vertex[j++] = 0.6f * cos(i);
    	vertex[j++] = -chill_height / 2;
    	vertex[j++] = 0.6f * sin(i);
    	vertex[j++] = nx;
    	vertex[j++] = ny;
    	vertex[j++] = nz;
    	vertex[j++] = 0.6f * cos(i + M_PI*APPROX);
    	vertex[j++] = -chill_height / 2;
    	vertex[j++] = 0.6f * sin(i + M_PI*APPROX);
    	vertex[j++] = nx;
    	vertex[j++] = ny;
    	vertex[j++] = nz;
    	vertex[j++] = 0.6f * cos(i);
    	vertex[j++] = chill_height / 2;
    	vertex[j++] = 0.6f * sin(i);
    	vertex[j++] = nx;
    	vertex[j++] = ny;
    	vertex[j++] = nz;
	}
	for(GLfloat i = 0.0f; i <= 2*M_PI; i += M_PI*APPROX){
    	GLfloat ax = cos(i) - cos(i + M_PI * APPROX);
    	GLfloat ay = 0.0f;
    	GLfloat az = sin(i) - sin(i + M_PI*APPROX);
    	GLfloat bx = 0.0f;
    	GLfloat by = 1.0f;
    	GLfloat bz = 0.0f;
    	GLfloat nx = ay * bz - az * by;
    	GLfloat ny = 0.0f;
    	GLfloat nz = ax*by - ay*bx;
    	vertex[j++] = 0.6f * cos(i);
    	vertex[j++] = chill_height / 2;
    	vertex[j++] = 0.6f * sin(i);
    	vertex[j++] = nx;
    	vertex[j++] = ny;
    	vertex[j++] = nz;
    	vertex[j++] = 0.6f * cos(i + M_PI*APPROX);
    	vertex[j++] = chill_height / 2;
    	vertex[j++] = 0.6f * sin(i + M_PI*APPROX);
    	vertex[j++] = nx;
    	vertex[j++] = ny;
    	vertex[j++] = nz;
    	vertex[j++] = 0.6f * cos(i + M_PI*APPROX);
    	vertex[j++] = -chill_height / 2;
    	vertex[j++] = 0.6f * sin(i + M_PI*APPROX);
    	vertex[j++] = nx;
    	vertex[j++] = ny;
    	vertex[j++] = nz;
	}
	return vertex;
}

int main()
{
	GLfloat AmbStren, DifStren, SpecStren;
	std::cout << "Ambient light rate:\n";
	std::cin >> AmbStren;
	std::cout << "Diffuse light rate:\n";
	std::cin >> DifStren;
	std::cout << "Specular light rate:\n";
	std::cin >> SpecStren;
	std::cout << "Precision of figure:\n";
	std::cin >> APPROX;
	glm::vec3 strenght(AmbStren, DifStren, SpecStren);
	glfwInit();
	glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
	glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
	glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);
	glfwWindowHint(GLFW_RESIZABLE, GL_TRUE);
	glfwWindowHint(GLFW_SAMPLES, 4);
	window = glfwCreateWindow(WIDTH, HEIGHT, "Лабораторная работа 4-5 Степанов Д.М. 307Б a-поворот вокруг ox, w-поворот вокруг oy, d-поворот вокруг oz, h-удаление невидимых линий", nullptr, nullptr);
	glfwMakeContextCurrent(window);
	glfwSetKeyCallback(window, key_callback);
	glfwSetCursorPosCallback(window, mouse_callback);
	glfwSetFramebufferSizeCallback(window, new_func_size_callback);
	glewExperimental = GL_TRUE;
	glewInit();
	glViewport(0, 0, WIDTH, HEIGHT);
	glEnable(GL_DEPTH_TEST);
	glEnable(GL_MULTISAMPLE);
	Shader lightingShader("shaders/lighting.vs", "shaders/lighting.frag");
	Shader lampShader("shaders/lamp.vs", "shaders/lamp.frag");
	GLfloat* vertices = get_figure();
	GLuint VBO, containerVAO;
	glGenVertexArrays(1, &containerVAO);
	glGenBuffers(1, &VBO);
	glBindBuffer(GL_ARRAY_BUFFER, VBO);
	glBufferData(GL_ARRAY_BUFFER, sizeof(GLfloat) * (1 + (unsigned)(2/APPROX)) * 9 * 4 * 2, vertices, GL_DYNAMIC_DRAW);
	glBindVertexArray(containerVAO);
	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(GLfloat), (GLvoid*)0);
	glEnableVertexAttribArray(0);
	glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(GLfloat), (GLvoid*)(3 * sizeof(GLfloat)));
	glEnableVertexAttribArray(1);
	glBindVertexArray(0);
	GLuint lightVAO;
	glGenVertexArrays(1, &lightVAO);
	glBindVertexArray(lightVAO);
	glBindBuffer(GL_ARRAY_BUFFER, VBO);
	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 3 * sizeof(GLfloat), (GLvoid*)0);
	glEnableVertexAttribArray(0);
	glBindVertexArray(0);

	glm::mat4 chillModel = glm::mat4(1.0f);
	load_state();
	load = true;
	while (!glfwWindowShouldClose(window))
	{
    	GLfloat currentFrame = glfwGetTime();
    	deltaTime = currentFrame - lastFrame;
    	lastFrame = currentFrame;
    	glfwPollEvents();
    	do_movement();
    	glClearColor(0.0f, 0.0f, 0.0f, 0.003f);
    	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
    	lightingShader.Use();
    	GLint objectColorLoc = glGetUniformLocation(lightingShader.Program, "objectColor");
    	GLint lightColorLoc  = glGetUniformLocation(lightingShader.Program, "lightColor");
    	GLint lightPosLoc	= glGetUniformLocation(lightingShader.Program, "lightPos");
    	GLint viewPosLoc 	= glGetUniformLocation(lightingShader.Program, "viewPos");
    	GLint strenPosLoc = glGetUniformLocation(lightingShader.Program, "strength");
    	GLuint ShinessPosLoc = glGetUniformLocation(lightingShader.Program, "shiness");
    	GLuint LightSourceColourPosLoc = glGetUniformLocation(lightingShader.Program, "shiness");
    	glUniform3f(objectColorLoc, 0.15f, 0.96f, 0.37f);
    	glUniform1f(ShinessPosLoc, shiness);
    	glUniform3f(lightColorLoc,  LightSourceColour.x, LightSourceColour.y, LightSourceColour.z);
    	glUniform3f(lightPosLoc,	lightPos.x, lightPos.y, lightPos.z);
    	glUniform3f(viewPosLoc, 	camera.Position.x, camera.Position.y, camera.Position.z);
    	glUniform3f(strenPosLoc, 	strenght.x, strenght.y, strenght.z);
    	glm::mat4 view;
    	view = camera.GetViewMatrix();
    	glm::mat4 projection = glm::perspective(camera.Zoom, (GLfloat)WIDTH / (GLfloat)HEIGHT, 0.1f, 100.0f);
    	GLint modelLoc = glGetUniformLocation(lightingShader.Program, "model");
    	GLint viewLoc  = glGetUniformLocation(lightingShader.Program,  "view");
    	GLint projLoc  = glGetUniformLocation(lightingShader.Program,  "projection");
    	glUniformMatrix4fv(viewLoc, 1, GL_FALSE, glm::value_ptr(view));
    	glUniformMatrix4fv(projLoc, 1, GL_FALSE, glm::value_ptr(projection));
    	glBindVertexArray(containerVAO);
    	glm::mat4 model = glm::mat4(1.0f);

    	if (ret) {
        	chillModel = glm::mat4(1.0f);
        	ret = false;
    	} else{
        	glm::mat4 rotation;
        	glm::mat4 scalem = glm::scale(glm::mat4(1.0f), glm::vec3(scale, scale, scale));
        	if (load) {
            	glm::mat4 x_rotation_mat = glm::rotate(glm::mat4(1.0f), x_rotation_load, glm::vec3(1.0f, 0.0f, 0.0f));
            	glm::mat4 y_rotation_mat = glm::rotate(glm::mat4(1.0f), y_rotation_load, glm::vec3(0.0f, 1.0f, 0.0f));
            	glm::mat4 z_rotation_mat = glm::rotate(glm::mat4(1.0f), z_rotation_load, glm::vec3(0.0f, 0.0f, 1.0f));
            	rotation = x_rotation_mat * y_rotation_mat * z_rotation_mat;
            	chillModel = rotation * scalem * chillModel;
            	load = false;
        	} else {
            	if (change) {
                	glm::mat4 x_rotation_mat = glm::rotate(glm::mat4(1.0f), x_rotation, glm::vec3(1.0f, 0.0f, 0.0f));
                	glm::mat4 y_rotation_mat = glm::rotate(glm::mat4(1.0f), y_rotation, glm::vec3(0.0f, 1.0f, 0.0f));
                	glm::mat4 z_rotation_mat = glm::rotate(glm::mat4(1.0f), z_rotation, glm::vec3(0.0f, 0.0f, 1.0f));
                	rotation = x_rotation_mat * y_rotation_mat * z_rotation_mat;
                	chillModel = rotation * scalem * chillModel;
            	}
        	}
    	}
    	glUniformMatrix4fv(modelLoc, 1, GL_FALSE, glm::value_ptr(chillModel));
    	glDrawArrays(GL_TRIANGLES, 0, (1 + (unsigned)(2/APPROX)) * 3 * 4);
    	lampShader.Use();
    	modelLoc = glGetUniformLocation(lampShader.Program, "model");
    	viewLoc  = glGetUniformLocation(lampShader.Program, "view");
    	projLoc  = glGetUniformLocation(lampShader.Program, "projection");
    	glUniformMatrix4fv(viewLoc, 1, GL_FALSE, glm::value_ptr(view));
    	glUniformMatrix4fv(projLoc, 1, GL_FALSE, glm::value_ptr(projection));
    	model = glm::mat4(1.0);
    	model = glm::translate(model, lightPos);
    	model = glm::scale(model, glm::vec3(0.2f));
    	glUniformMatrix4fv(modelLoc, 1, GL_FALSE, glm::value_ptr(model));
    	glBindVertexArray(lightVAO);
    	glDrawArrays(GL_TRIANGLES, 0, 36);
    	glBindVertexArray(0);
    	glfwSwapBuffers(window);
	}
	glfwTerminate();
	free(vertices);
	save_state();
	return 0;
}

void key_callback(GLFWwindow* window, int key, int scancode, int action, int mode)
{
	if (key == GLFW_KEY_ESCAPE && action == GLFW_PRESS) glfwSetWindowShouldClose(window, GL_TRUE);
	if (key >= 0 && key < 1024) {
    	if (action == GLFW_PRESS)
        	keys[key] = true;
    	else if (action == GLFW_RELEASE)
        	keys[key] = false;
	}
}

void do_movement()
{
	if (keys[GLFW_KEY_W]) camera.ProcessKeyboard(FORWARD, deltaTime);
	if (keys[GLFW_KEY_S]) camera.ProcessKeyboard(BACKWARD, deltaTime);
	if (keys[GLFW_KEY_A]) camera.ProcessKeyboard(LEFT, deltaTime);
	if (keys[GLFW_KEY_D]) camera.ProcessKeyboard(RIGHT, deltaTime);
	if (keys[GLFW_KEY_Q]) glfwSetInputMode(window, GLFW_CURSOR, GLFW_CURSOR_DISABLED);
	if (keys[GLFW_KEY_Z]) glfwSetInputMode(window, GLFW_CURSOR, GLFW_CURSOR_NORMAL);
	if (keys[GLFW_KEY_UP]) {
    	change = true;
    	scale += 0.05f;
	} else if (keys[GLFW_KEY_DOWN]) {
    	change = true;
    	scale -= 0.05f;
	} else {
    	scale = 1.0f;
	}
	if (keys[GLFW_KEY_LEFT]) {
    	--shiness;
	} else if (keys[GLFW_KEY_RIGHT]) {
    	++shiness;
	}
	if (keys[GLFW_KEY_1]) {
    	if (LightSourceColour.x > 0) LightSourceColour.x -= 0.09f;
	} else if (keys[GLFW_KEY_2]) {
    	if (LightSourceColour.x < 1) LightSourceColour.x += 0.09f;
	} else if (keys[GLFW_KEY_3]) {
    	if (LightSourceColour.y > 0) LightSourceColour.y -= 0.09f;
	} else if (keys[GLFW_KEY_4]) {
    	if (LightSourceColour.y < 1) LightSourceColour.y += 0.09f;
	} else if (keys[GLFW_KEY_5]) {
    	if (LightSourceColour.z > 0) LightSourceColour.z -= 0.09f;
	} else if (keys[GLFW_KEY_6]) {
    	if (LightSourceColour.z < 1) LightSourceColour.z += 0.09f;
	}

	if (keys[GLFW_KEY_Y]) {
    	change = true;
    	x_rotation = glm::radians(5.0f);
    	x_rotation_save = x_rotation;
	} else if (keys[GLFW_KEY_T]) {
    	change = true;
    	x_rotation = -glm::radians(5.0f);
    	x_rotation_save = x_rotation;
	} else {
    	if (change) x_rotation = 0.0f;
	}
	if (keys[GLFW_KEY_G]) {
    	change = true;
    	y_rotation = -glm::radians(5.0f);
    	y_rotation_save = y_rotation;
	} else if (keys[GLFW_KEY_H]) {
    	change = true;
    	y_rotation = glm::radians(5.0f);
    	y_rotation_save = y_rotation;
	} else {
    	if (change) y_rotation = 0.0f;
	}
	if (keys[GLFW_KEY_B]) {
    	change = true;
    	z_rotation = -glm::radians(5.0f);
    	z_rotation_save = z_rotation;
	} else if (keys[GLFW_KEY_N]) {
    	change = true;
    	z_rotation = glm::radians(5.0f);
    	z_rotation_save = z_rotation;
	} else {
    	if (change) z_rotation = 0.0f;
	}
	if (keys[GLFW_KEY_R]) {
    	ret = true;
	}
	if (keys[GLFW_KEY_L]) {
    	glPolygonMode(GL_FRONT_AND_BACK, GL_LINE);
	} else if (keys[GLFW_KEY_K]) {
    	glPolygonMode(GL_FRONT_AND_BACK, GL_FILL);
	}
}

bool firstMouse = true;
void mouse_callback(GLFWwindow* window, double xpos, double ypos)
{
	if (firstMouse)
	{
    	lastX = xpos;
    	lastY = ypos;
    	firstMouse = false;
	}
	GLfloat yaw   = -90.0f;
	GLfloat pitch = 0.0f;
	GLfloat xoffset = xpos - lastX;
	GLfloat yoffset = lastY - ypos;
	lastX = xpos;
	lastY = ypos;
	GLfloat sentivity = 0.15f;
	xoffset *= sentivity;
	yoffset *= sentivity;
	yaw   += xoffset;
	pitch += yoffset;
	if(pitch > 89.0f) pitch =  89.0f;
	if(pitch < -89.0f) pitch = -89.0f;
	glm::vec3 front;
	front.x = cos(glm::radians(pitch)) * cos(glm::radians(yaw));
	front.y = sin(glm::radians(pitch));
	front.z = cos(glm::radians(pitch)) * sin(glm::radians(yaw));
	cameraFront = glm::normalize(front);
	camera.ProcessMouseMovement(xoffset, yoffset);
}

void scroll_callback(GLFWwindow* window, double xoffset, double yoffset)
{
	camera.ProcessMouseScroll(yoffset);
}

void save_state()
{
   tinyxml2::XMLDocument state;

    tinyxml2::XMLElement *root = state.NewElement("state"); {
  	 tinyxml2::XMLElement *rotation = state.NewElement("rotation"); {
 		 tinyxml2::XMLElement *x = state.NewElement("x");
     	x->SetText(std::to_string(x_rotation_save).c_str());
 		 
 		 tinyxml2::XMLElement *y = state.NewElement("y");
     	y->SetText(std::to_string(y_rotation_save).c_str());

 		 tinyxml2::XMLElement *z = state.NewElement("z");
     	z->SetText(std::to_string(z_rotation_save).c_str());

 		 rotation->InsertEndChild(x);
 		 rotation->InsertEndChild(y);
 		 rotation->InsertEndChild(z);
  	 } root->InsertEndChild(rotation);

   
  	 tinyxml2::XMLElement *load_saved = state.NewElement("load-saved");
  	 load_saved->SetText("saved");
  	 root->InsertEndChild(root);
	state.InsertFirstChild(root);

	} state.SaveFile("./state.xml");
}

void load_state() {
    tinyxml2::XMLDocument state;

    if (state.LoadFile("state.xml") == tinyxml2::XML_SUCCESS) {
  	 tinyxml2::XMLElement *root = state.RootElement();
	std::cout << "yes\n";
  	tinyxml2::XMLElement *rotation = root->FirstChildElement("rotation");
  	std::cout << "load:\n";
  	x_rotation_load = rotation->FirstChildElement("x")->FloatText();
  	y_rotation_load = rotation->FirstChildElement("y")->FloatText();
  	z_rotation_load = rotation->FirstChildElement("z")->FloatText();
  	std::cout << x_rotation_load << " " << y_rotation_load << " " << z_rotation_load << "\n";
    }
}
