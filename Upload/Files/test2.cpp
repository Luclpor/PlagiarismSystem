include<iostream>
using namespace std;
#define DBL_MAX 1.7976931348623158e+308
int n;
double OPTIMAL_BST(double *p,double *q,int n,double **e,double **w,int **root)
{
	for(int i=1;i<=n+1;i++)
	{
		e[i][i-1]=q[i-1];
		w[i][i-1]=q[i-1];
	}
	for(int l=1;l<=n;l++)
	{
		for(int i=1;i<=n-l+1;i++)
		{	
			int j=i+l-1;
			e[i][j]=DBL_MAX ;
			w[i][j]=w[i][j-1]+p[j]+q[j];
			for(int r=i;r<=j;r++)
			{
				double t=e[i][r-1]+e[r+1][j]+w[i][j];
				
				if(t<e[i][j])
				{
					e[i][j]=t;
					root[i][j]=r;
				}
			}
		}
	}
	return e[1][n];
}

int main()
{
	cin>>n;
	double p[n+1];
	double q[n+1];
	p[0]=0;
	for(int i=1; i<=n; i++) cin>>p[i];
	for(int i=0; i<n+1; i++) cin>>q[i];
	double **e=new double *[n+2];
	for(int i=0;i<=n+1;i++) e[i]=new double[n+1];
	double **w=new double *[n+2];
	for(int i=0;i<=n+1;i++) w[i]=new double[n+1];
	int **root=new int *[n+1];
	for(int i=0;i<=n;i++) root[i]=new int[n+1];
	cout<<OPTIMAL_BST(p,q,n,e,w,root)<<endl;
	for (int i = 0; i < n+2; i++)     
    {  
        delete e[i],delete w[i];    
        e[i] = NULL,w[i] = NULL;
		if(i!=n+1)
		{
			delete root[i];
			root[i] = NULL;
		}
    }  
	delete []e,delete[]w,delete []root; 
    e = NULL,w=NULL,root=NULL;
	return 0;
}