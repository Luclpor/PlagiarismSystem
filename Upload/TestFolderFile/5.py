import numpy as np
import math
import matplotlib.pyplot as plt
from matplotlib.widgets import TextBox

fig, ax = plt.subplots()
plt.subplots_adjust(bottom=0.2)


def submit(text):  # функция ввода параметров a и b
    a, b = eval(text)
    fig1, ax1 = plt.subplots()
    plt.subplots_adjust(bottom=0.2)
    t = np.arange(-math.pi - 1, math.pi + 1, 0.01)
    x = []
    y = []
    for i in range(len(t)):
        x.append(a * math.sin(t[i]))
        y.append(b * math.cos(t[i]))
    initial_text = "some func"
    (l,) = plt.plot(x, y, lw=2)

    plt.draw()
    plt.show()


axbox = plt.axes([0.1, 0.05, 0.8, 0.075])
text_box = TextBox(axbox, "Evaluate")  # окно ввода
text_box.on_submit(submit)

plt.show()
