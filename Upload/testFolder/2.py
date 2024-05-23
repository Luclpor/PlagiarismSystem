import matplotlib.pyplot as plt
import numpy as np
import sympy as sp

phi = sp.Symbol('phi')
a = 1
x = a * sp.cos(phi) * sp.cos(phi) * sp.cos(phi)
y = a * sp.sin(phi) * sp.sin(phi) * sp.sin(phi)
T = np.linspace(-5, 5, 1000)

X = np.zeros_like(T)
Y = np.zeros_like(T)

for i in np.arange(len(T)):
   X[i] = sp.Subs(x, phi, T[i])
   Y[i] = sp.Subs(y, phi, T[i])

y = np.random.randint(-100, 100, size=(40))
x = np.arange(-20, 20)
ax1 = plt.gca()

#ARROWS
dxY = 0.1 * Y.max()
dyY = 0.1 * Y.max()
dxX = 0.1 * X.max()
dyX = 0.1 * X.max()
ax1.arrow(x=0, y=Y.max(), dx=-dxY, dy=-dyY)
ax1.arrow(x=0, y=Y.max(), dx=dxY, dy=-dyY)

ax1.arrow(x=X.max(), y=0, dx=-dxX, dy=dyX)
ax1.arrow(x=X.max(), y=0, dx=-dxX, dy=-dyX)

ax1.axhline(y=0, color='k')
ax1.axvline(x=0, color='k')

ax1.plot(X, Y)
plt.show()
