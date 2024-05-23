import numpy as np  # Вычисления
import matplotlib.pyplot as plt  # строит график
from matplotlib.widgets import TextBox  # обеспечивает ввод
import math  # математика


def get_coord(a):  # расчет координат
    phi = np.arange(0, 2 * np.pi, 0.01)  # полярный угол фи
    x = a * (np.cos(phi) ** 3)  # рассчитываем икс
    y_up = a * (np.sin(phi) ** 3)  # рассчитываем игрик в верхней полуплоскости
    x = np.concatenate((x, x))  # дублируем икс для нижней полуплоскости
    y = np.concatenate((y_up, -y_up))  # дублируем игрик для нижней полуплоскости
    return x, y


def submit(text):  # обработчик ввода данных в поле TextBox
    a = 6
    try:  # проверяем что ввели число
        a = float(text_box_a.text)
    except ValueError:
        print("Это не число!")
    x, y = get_coord(a)
    ax.set_ylim(
        -math.fabs(a) - math.fabs(a) / 10, math.fabs(a) + math.fabs(a) / 10
    )  # обновляем ограничения осей по Х и по У
    ax.set_xlim(-math.fabs(a) - math.fabs(a) / 10, math.fabs(a) + math.fabs(a) / 10)
    l.set_xdata(x)
    l.set_ydata(y)
    plt.draw()


fig = plt.figure("Декартова система - астроида", figsize=(8.0, 8.0))
ax = fig.add_subplot(111)
ax.set_xlim(-10, 10)
plt.subplots_adjust(bottom=0.2)
plt.title("x = a*cos(phi)^3, y = a*sin(phi)^3")
a = 6  # начальное значение
x, y = get_coord(a)
initial_text = "6"  # начальное значение для TextBox
print(x, y)
(l,) = plt.plot(x, y, "r", color="blue")
ax.set_ylim(
    -math.fabs(a) - 1, math.fabs(a) + 1
)  # обновляем ограничения осей по Х и по У
ax.set_xlim(-math.fabs(a) - 1, math.fabs(a) + 1)

ax_box = plt.axes([0.6, 0.1, 0.05, 0.040])  # начальные координаты текстбокса
text_box_a = TextBox(
    ax_box, "Enter the value of parameter a for func:", initial=initial_text
)
text_box_a.on_submit(submit)
plt.show()
