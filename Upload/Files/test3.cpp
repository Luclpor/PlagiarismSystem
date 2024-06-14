#include<bits/stdc++.h>
typedef long long ll;
using namespace std;
int n,m;ll K;
const int N=110000;
vector<int> A[3],B[3];
ll get(ll lim)
{
        if (lim>=0)
        {
                ll ans=(ll)(A[1].size())*m+(ll)(B[1].size())*(n-A[1].size());
                ans+=1ll*A[0].size()*B[2].size()+1ll*A[2].size()*B[0].size();
                for (int a=0;a<=2;a+=2)
                {
                        int b=a;
                        int now=int(B[b].size())-1;
                        for (int i=0;i<A[a].size();++i)
                        {
                                while (now>=0&&1ll*A[a][i]*B[b][now]>lim)
                                        now--;
                                ans+=now+1;
                        }
                }
                //cout<<""1 ""<<ans<<endl;
                return ans;
        }
        else if (lim<0)
        {
                lim*=-1;
                ll ans=0;
                for (int a=0;a<=2;a+=2)
                {
                        int b=2-a;
                        int now=int(B[b].size())-1;
                        for (int i=0;i<A[a].size();++i)
                        {
                                while (now>=0&&1ll*A[a][i]*B[b][now]>=lim)
                                        now--;
                                ans+=int(B[b].size())-now-1;
                        }
                }
                //cout<<""2: ""<<ans<<endl;
                return ans;
        }
}
int main()
{
        scanf("%d%d%lld",&n,&m,&K);
        K=1ll*n*m-K+1;
        for (int i=1;i<=n;i++)
        {
                int k1; scanf("%d",&k1);
                if (k1>0) A[0].push_back(k1);
                else if (k1==0) A[1].push_back(k1);
                else A[2].push_back(-k1);
        }
        for (int i=1;i<=m;i++)
        {
                int k1; scanf("%d",&k1);
                if (k1>0) B[0].push_back(k1);
                else if (k1==0) B[1].push_back(k1);
                else B[2].push_back(-k1);
        }
        for (int i=0;i<3;i++)
        {
                sort(A[i].begin(),A[i].end());
                sort(B[i].begin(),B[i].end());
        }
        ll l=0,r=2e12,ans=0,bias=1e12;
        while (l<=r)
        {
                ll mid=(l+r>>1);
                ll qwq=get(mid-bias);
                //cout<<qwq<<endl;
                if (qwq>=K)
                {
                        ans=mid-bias;
                        r=mid-1;
                }
                else
                        l=mid+1;
        }
        cout<<ans<<endl;
}
/*
1 3 3
-2
4 2 -2
*/