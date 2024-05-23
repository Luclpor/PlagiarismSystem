import numpy as np
import math
import matplotlib.pyplot as plt
from matplotlib.widgets import TextBox
fig = plt.figure(figsize=(8, 8))
ax = fig.add_axes([0.1, 0.1, 0.8, 0.8], projection=&quot;polar&quot;)
plt.text(0, 1, &quot;функция: ρ = a*φ, 0&lt;=φ&lt;=B&quot;, fontsize=10,
transform=ax.transAxes)

def submit(number):
ax.cla()
plt.text(0, 1, &quot;функция: ρ = a*φ, 0&lt;=φ&lt;=B&quot;, fontsize=10,
transform=ax.transAxes)
a,B= eval(number)
phi = np.arange(0, math.radians(B), 0.01)
r = a * phi
ax.plot(phi, r, color=&quot;red&quot;)
axboxA = plt.axes([0.3, 0.05, 0.5, 0.035])
text_boxA = TextBox(axboxA, &#39;Введите a и B через запятую:&#39;)
text_boxA.on_submit(submit)
plt.show()