import numpy as np
from matplotlib import pyplot as plt
from math import pi
 
 
def main():
	u = 0
	v = 0
	a = 5
	b = 5
	t = np.linspace(0, 2 * pi, 100)
 
	plt.annotate("", xy=(10.1, 0), xycoords='data', xytext=(-10.1, 0), textcoords='data',
                 arrowprops=dict(arrowstyle="->", connectionstyle="arc3"))
	plt.annotate("", xy=(0, 10.1), xycoords='data', xytext=(0, -10.1), textcoords='data',
                 arrowprops=dict(arrowstyle="->", connectionstyle="arc3"))
	plt.grid(color='lightgray', linestyle='--')
	plt.plot(0, 0, 'bo')
 
	plt.text(9.6, 0.5, 'x')
	plt.text(0.5, 9.6, 'y')
 
	plt.xlim(-10.1, 10.1)
	plt.ylim(-10.1, 10.1)
 
	plt.plot(u + a * np.cos(t), v + b * np.sin(t))
 
	plt.show()
 
if __name__ == '__main__':
	main()
