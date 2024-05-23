#ifndef UNICODE
#define UNICODE
#endif

#define NTDDI_WIN10_19H1 (0x0A000007)

#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <windows.h>
#include <CommCtrl.h>

#define CWIDTH  2
#define WIDTH   1280
#define HEIGHT  720
#define LINES   10
#define EDTID1  200
#define BTTN1  10
#define BTTN2  11

COLORREF rgbGreen = 0x00FF00;
COLORREF rgbRed   = 0x0000FF;
COLORREF rgbBlack = 0x000000;
COLORREF rgbWhite = 0xFFFFFF;

typedef enum {
    GREEN = 0, RED, BLACK, WHITE
} Colors, *Colors_ptr;

typedef struct null_kords {
    double x0;
    double y0;
} zero_axis, *zero_axis_ptr;

typedef struct object_kords {
    double x;
    double y;
    Colors axis_x;
    Colors axis_y;
    zero_axis kords;
} Axis, *Axis_ptr;

typedef struct object_graph {
    double  a;
    long long    size;
    double  start;
    double  end;
    Colors  col;
    double  scale;
} graph, *graph_ptr;

RECT clientsize;
LPRECT client_window = &clientsize;

Axis    kords = {0, 0};
graph   draw_graph = {0};
int     sectorZ = HEIGHT / 20;
HWND    simple_text;
int     wParamValueA = 0;
wchar_t szBuf[100];
int     nSize;
int     rangea = -5;
int     rangeb = 5;
long long    magicnum  = 100;
HWND    edt1_field;
HWND    hwndTrack;
HWND    bttnplus;
HWND    bttnminus;

unsigned int height = HEIGHT;
unsigned int width = WIDTH;

LRESULT WindowProc(HWND hwnd, UINT message, WPARAM wparam, LPARAM lparam);
Axis    InitAxis(double x, double y, COLORREF col_x, COLORREF col_y, double x0, double y0);
graph   InitGraph(double a, COLORREF col, double start, double end, double scale);
void    WinPaint(HDC hdc);
void    RenderObject(HDC hdc);
void    Line(HDC hdc, int x1, int y1, int x2, int y2, Colors c);

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR pCmdLine, int nCmdShow) {
    WNDCLASS wc = {0};

    wc.hInstance     = hInstance;
    wc.lpfnWndProc   = WindowProc;
    wc.lpszClassName = TEXT("Sokolov 307 8 variant lab01");
    wc.hCursor = LoadCursor(NULL, IDC_ARROW);
    RegisterClass(&wc);

    HWND hwnd = CreateWindowEx( 0, wc.lpszClassName, (LPCWSTR) "Sokolov lab01 var. 8",
                                WS_OVERLAPPEDWINDOW, 50, 50, width, height, NULL, NULL, hInstance, NULL );
    
    GetClientRect(hwnd, client_window);

    ShowWindow(hwnd, nCmdShow);
    UpdateWindow(hwnd);

    hwndTrack = CreateWindowEx(0, TRACKBAR_CLASS, TEXT("trackbar"), WS_CHILD | WS_VISIBLE | BS_PUSHBUTTON | TBS_ENABLESELRANGE, 
            0, client_window->bottom - sectorZ, (client_window->right - client_window->left) / 3, sectorZ, hwnd, (HMENU) 1, hInstance, NULL);
    
    SendMessage(hwndTrack, TBM_SETRANGE, (WPARAM) TRUE, (LPARAM) MAKELONG(-100, 1000));

    simple_text = CreateWindowEx(0, WC_STATIC, TEXT("scale = 1.000000"), WS_CHILD | WS_VISIBLE | BS_PUSHBUTTON, 
            (client_window->right - client_window->left) / 3, client_window->bottom - sectorZ, (client_window->right - client_window->left) / 6, sectorZ, hwnd, (HMENU) 1, hInstance, NULL);
    
    edt1_field = CreateWindowEx(0, WC_EDIT, TEXT("1.00000"), WS_CHILD | WS_VISIBLE | WS_BORDER | WS_EX_RIGHT | ES_NUMBER, 
            (client_window->right - client_window->left) / 2, client_window->bottom - sectorZ, (client_window->right - client_window->left) / 6, sectorZ, hwnd, (HMENU) EDTID1, hInstance, NULL);
    
    bttnplus = CreateWindowEx(0, WC_BUTTON, TEXT("+"), WS_CHILD | WS_VISIBLE, 
            (client_window->right - client_window->left) / 2 + (client_window->right - client_window->left) / 6, client_window->bottom - sectorZ, (client_window->right - client_window->left) / 6, sectorZ, hwnd, (HMENU) BTTN1, hInstance, NULL);
    
    bttnminus = CreateWindowEx(0, WC_BUTTON, TEXT("-"), WS_CHILD | WS_VISIBLE, 
            (client_window->right - client_window->left) / 2 + (client_window->right - client_window->left) / 6 + (client_window->right - client_window->left) / 6, client_window->bottom - sectorZ, (client_window->right - client_window->left) / 6, sectorZ, hwnd, (HMENU) BTTN2, hInstance, NULL);
    

    HDC hdc = GetDC(hwnd);

    kords = InitAxis(0, 0, RED, GREEN, (client_window->right - client_window->left) / 2, (client_window->bottom - client_window->top - sectorZ) / 2);
    draw_graph = InitGraph(1, WHITE, rangea, rangeb, 1);

    printf("koords: x = %lf, y = %lf\n", kords.kords.x0, kords.kords.y0);

    MSG msg = {0};
    while (1) {
        if (PeekMessageA(&msg, NULL, 0, 0, PM_REMOVE)) {
            if (msg.message == WM_QUIT)
                break;

            TranslateMessage(&msg);
            DispatchMessage(&msg);
        }

        WinPaint(hdc);

    }


    return 0;
}

LRESULT WindowProc(HWND hwnd, UINT message, WPARAM wparam, LPARAM lparam) {
    if (message == WM_CLOSE) {
        DestroyWindow(hwnd);
    } else if (message == WM_DESTROY) {
        PostQuitMessage(0);
    } else if (message == WM_CREATE) {
        
    } else if (message == WM_HSCROLL) {
        printf("WM_HSCROLL\n");
        int k = HIWORD(wparam);
        printf("k: %d\n", k);

        if (k > 1000) {
            wParamValueA = k - 65536;
            printf("wParamValueA - k: %d\n", k - 65536);
            wParamValueA = k - 65536;
        } else {
            k != 0 ? wParamValueA = k : 0;
        }

        if (k == 0) {
            if (LOWORD(wparam) == SB_LINELEFT) {
                wParamValueA == -100 ? wParamValueA = -100 : --wParamValueA;
                
            } else if (LOWORD(wparam) == SB_LINERIGHT) {
                if (wParamValueA < 1000) 
                    wParamValueA += 1;
            }
        } else {
            wParamValueA > 0 ? wParamValueA = k : 0;
        }

        draw_graph.scale = 1 + wParamValueA / 100.f;
        if (draw_graph.scale <= 1)
            draw_graph.size = 1 + magicnum / draw_graph.scale * 100;

        nSize = swprintf(szBuf, 100, TEXT("scale = %lf"), draw_graph.scale);
        SetWindowText(simple_text, szBuf);
        printf("wParamValue: %d\n", wParamValueA);
        printf("size: %d\n", draw_graph.size);
    } else if (message == WM_COMMAND) {
        if (LOWORD(wparam) == EDTID1) {
            int nc = HIWORD(wparam);
            char c[100];

            GetWindowTextA(edt1_field, c, 100);
            if (nc == EN_UPDATE || nc == EN_CHANGE) {
                draw_graph.a = atoi(c);
                printf("a: %f\n", draw_graph.a);
            }
        } else if (LOWORD(wparam) == BTTN1) {
            if (rangeb < 100) {
                rangea -= 5;
                rangeb += 5;
            }
        } else if (LOWORD(wparam) == BTTN2) {
            if (rangeb > 5) {
                rangea += 5;
                rangeb -= 5;
            }
        }

        magicnum = (rangeb - rangea) * LINES + 1;
        draw_graph = InitGraph(1, WHITE, rangea, rangeb, 1);
    } else if (message == WM_SIZE) {
        height = HIWORD(lparam);
        width = LOWORD(lparam);

        GetClientRect(hwnd, client_window);
        kords = InitAxis(0, 0, RED, GREEN, (client_window->right - client_window->left) / 2, (client_window->bottom - client_window->top - sectorZ) / 2);
        SetWindowPos(hwndTrack, HWND_TOP, 0, client_window->bottom - sectorZ, (client_window->right - client_window->left) / 3, sectorZ, SWP_SHOWWINDOW);
        SetWindowPos(simple_text, HWND_TOP, (client_window->right - client_window->left) / 3, client_window->bottom - sectorZ, (client_window->right - client_window->left) / 6, sectorZ, SWP_SHOWWINDOW);
        SetWindowPos(edt1_field, HWND_TOP, (client_window->right - client_window->left) / 2, client_window->bottom - sectorZ, (client_window->right - client_window->left) / 6, sectorZ, SWP_SHOWWINDOW);
        SetWindowPos(bttnplus, HWND_TOP, (client_window->right - client_window->left) / 2 + (client_window->right - client_window->left) / 6, client_window->bottom - sectorZ, (client_window->right - client_window->left) / 6, sectorZ, SWP_SHOWWINDOW);
        SetWindowPos(bttnminus, HWND_TOP, (client_window->right - client_window->left) / 2 + (client_window->right - client_window->left) / 6 + (client_window->right - client_window->left) / 6, client_window->bottom - sectorZ, (client_window->right - client_window->left) / 6, sectorZ, SWP_SHOWWINDOW);
        
    } else if (message == WM_KEYDOWN) {

        if (wparam == VK_SHIFT) {
            printf("SHIFT\n");
        } else if (wparam == VK_CONTROL) {
            printf("CONTROL\n");
        }
        
    }else
        return DefWindowProcA(hwnd, message, wparam, lparam);
}

Axis InitAxis(double x, double y, COLORREF col_x, COLORREF col_y, double x0, double y0) {
    Axis tmp = {x, y, col_x, col_y, x0, y0};

    return tmp;
}

graph InitGraph(double a, COLORREF col, double start, double end, double scale) {
    graph tmp;

    tmp.a = a;
    tmp.col = col;
    tmp.scale = scale;
    tmp.size = (end - start) * LINES + 1;
    tmp.end = end;
    tmp.start = start;

    return tmp;
}

void WinPaint(HDC hdc) {
    RenderObject(hdc);

    // WinMoveAsteroid();
    // WinCheckLine();
}

void RenderObject(HDC hdc) {
    HDC memhdc = CreateCompatibleDC(hdc);
    HBITMAP hbmap = CreateCompatibleBitmap(hdc, client_window->right - client_window->left, client_window->bottom - client_window->top - sectorZ);

    SelectObject(memhdc, (HGDIOBJ) hbmap);
    SelectObject(memhdc, GetStockObject(DC_BRUSH));
    SetDCBrushColor(memhdc, RGB(0, 0, 0));
    Rectangle(memhdc, 0, 0, client_window->right, client_window->bottom);

    Line(memhdc, kords.x + kords.kords.x0, 0, kords.x + kords.kords.x0, client_window->bottom - client_window->top - sectorZ, kords.axis_x);
    Line(memhdc, 0, kords.y + kords.kords.y0, client_window->right - client_window->left, kords.y + kords.kords.y0, kords.axis_y);
    // Line(memhdc, 0, 0, 100, 100, WHITE);
    long double x, y, prev_x, prev_y;
    int wJLx = client_window->right - client_window->left;
    int wJLy = client_window->bottom - client_window->top - sectorZ;
    int hS = draw_graph.end - draw_graph.start;

    prev_x = draw_graph.start / LINES;
    prev_y = draw_graph.a * sqrt(pow(prev_x, 3));
    for (int i = 1; i < draw_graph.size; ++i) {
        x = draw_graph.start + (double) i / LINES;
        y = draw_graph.a * sqrt(pow(x, 3));

        long double x1 = x * draw_graph.scale;
        long double y1 = y * draw_graph.scale;

        //if (x > 0)
        //    Line(memhdc, prev_x * wJLx / hS + kords.kords.x0 + kords.x, kords.kords.y0 + kords.y - prev_y * wJLy / hS, x1 * wJLx / hS + kords.kords.x0 + kords.x, kords.kords.y0 + kords.y - y1 * wJLy / hS, draw_graph.col);
        
        if (x > 0 && y < client_window->bottom && prev_y < client_window->bottom)
            Line(memhdc, prev_x * draw_graph.scale * wJLx / hS + kords.kords.x0 + kords.x, kords.kords.y0 + kords.y - prev_y * draw_graph.scale * wJLy / hS, x1 * wJLx / hS + kords.kords.x0 + kords.x, kords.kords.y0 + kords.y - y1 * wJLy / hS, draw_graph.col);
        else if (y > client_window->bottom && prev_y > client_window->bottom && i < 5)
            Line(memhdc, 1 + kords.kords.x0, 1 + kords.kords.y0, 1 + kords.kords.x0, 0, WHITE);

        prev_x = x;
        prev_y = y;
    }

    // for (int j = 0; j < NUM_ASTEROIDS; ++j) {
    //     for (int i = 0; i < NUM_VERTEX - 1; ++i) {
    //         Line(memhdc, object[j].pos0.pos[0] + object[j].vert[i].pos[0], object[j].pos0.pos[1] - object[j].vert[i].pos[1], 
    //                 object[j].pos0.pos[0] + object[j].vert[i+1].pos[0], object[j].pos0.pos[1] - object[j].vert[i+1].pos[1], object[j].color);
    //     }

    //     Line(memhdc, object[j].pos0.pos[0] + object[j].vert[NUM_VERTEX - 1].pos[0], object[j].pos0.pos[1] - object[j].vert[NUM_VERTEX - 1].pos[1], 
    //             object[j].pos0.pos[0] + object[j].vert[0].pos[0], object[j].pos0.pos[1] - object[j].vert[0].pos[1], object[j].color);
    // }

    BitBlt(hdc, 0, 0, client_window->right - client_window->left, client_window->bottom - client_window->top, memhdc, 0, 0, SRCCOPY);

    DeleteObject(memhdc);
    DeleteObject(hbmap);
}

void Line(HDC hdc, int x1, int y1, int x2, int y2, Colors c) {
    MoveToEx(hdc, x1, y1, NULL);
    HPEN hpen;

    if (c == BLACK)
        hpen = CreatePen(PS_SOLID, CWIDTH, rgbBlack); 
    else if (c == GREEN)
        hpen = CreatePen(PS_SOLID, CWIDTH, rgbGreen);
    else if (c == RED)
        hpen = CreatePen(PS_SOLID, CWIDTH, rgbRed); 
    else if (c == WHITE)
        hpen = CreatePen(PS_SOLID, CWIDTH, rgbWhite);

    SelectObject(hdc, hpen);
    LineTo(hdc, x2, y2);
    
    DeleteObject(hpen);
};