include<iostream>
#include <iomanip>
using namespace std;
#define DBL_MAX 1.7976931348623158e+308

double OPTIMAL_BST(double *p,double *q,int n,double **e,double **w,int **root)
{
	for(int i=1;i<=n+1;i++)
	{
		e[i][i-1]=q[i-1];
		w[i][i-1]=q[i-1];
	}
	for(int l=1;l<=n;l++)         /* ???????? */
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
					//cout<<root[i][j]<<""\t""<<i<<"" ===============""<<j<<endl;
				}
			}
		}
	}
	return e[1][n];
}
void CONSTRUCT_OPTIAML_BST(int **root,int i,int j)
{
	//if(i==1 && j==n)printf(""K%d??\n"",root[i][j]);
	if(i < j)
	{
		//printf(""K%d?K%d????\n"",root[i][root[i][j]-1],root[i][j]);
		CONSTRUCT_OPTIAML_BST(root,i,root[i][j]-1);
		if(root[i][j]+1 < j) /* ?????????????root[5][4]?root[6][5] */
		//printf(""K%d?K%d????\n"",root[root[i][j]+1][j],root[i][j]);
		/* else  cout<<root[i][j]+1<<""\t""<<j<<endl; */
		CONSTRUCT_OPTIAML_BST(root,root[i][j]+1,j);
	}

	if(i == j)
	{
		//printf(""d%d?K%d????\n"",i-1,i);
		//printf(""d%d?K%d????\n"",i,i);
	}
	//if(i > j) printf(""d%d?K%d????\n"",j,j);
}
int main()
{
    int n;
    cin>>n;
    double p[n+2];
    double q[n+2];

    for(int i=1; i<=n; i++)
        cin>>p[i];
    for(int i=0; i<=n; i++)
        cin>>q[i];
	/* ???? */
	/* ?????e[1..n+1,0..n]  w[1..n+1,0..n] root[1..n,1..n] */
	double **e=new double *[n+2];
	for(int i=0;i<=n+1;i++) e[i]=new double[n+1];
	double **w=new double *[n+2];
	for(int i=0;i<=n+1;i++) w[i]=new double[n+1];
	int **root=new int *[n+1];
	for(int i=0;i<=n;i++) root[i]=new int[n+1];
	/* ???e?root */
	cout<<fixed<<setprecision(6)<<OPTIMAL_BST(p,q,n,e,w,root)<<endl;
	/* ??root */
	 CONSTRUCT_OPTIAML_BST(root,1,n);
	/* ???? */
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
}"