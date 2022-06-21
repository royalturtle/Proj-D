using GoogleMobileAds.Api;
using UnityEngine;

public class GoogleAdMobScript : MonoBehaviour
{
    private InterstitialAd interstitial;
    public void Start()
    {
        //���� �ʱ�ȭ
        MobileAds.Initialize(initStatus =>
        {
            RequestInterstitial();
        });
    }
    private void RequestInterstitial()
    {
        //���� OS���� ����� �ڵ带 ����� ��� �̷��� �ϸ� ��
            //���� ���� ID�� /�� �� ���� ���� ���� ID
            //�� ID���� Google�� �����ϴ� �׽�Ʈ ID�̹Ƿ� ���� ���� ��� ����
        #if UNITY_ANDROID
            // string adUnitId = "ca-app-pub-3940256099942544/1033173712";
            string adUnitId = "ca-app-pub-2254725980937464/4230700494";
        #elif UNITY_IPHONE
            // string adUnitId = "ca-app-pub-3940256099942544/4411468910";
            string adUnitId = "ca-app-pub-2254725980937464/5706307960";
        #else
            string adUnitId = "unexpected_platform";
        #endif
 
        //���� OS�� ��� ���⼭ �ٷ� ��Ʈ������ �Ⱦ��൵ ����
        this.interstitial = new InterstitialAd(adUnitId);
        AdRequest request = new AdRequest.Builder().Build();
        this.interstitial.LoadAd(request);
    }
 
    //���� �����ؾ� �� ���� �ܺο��� �� �Լ��� ȣ��
    public void AdStart()
    {
        if (this.interstitial.IsLoaded()) {
            this.interstitial.Show();
        }
    }
}