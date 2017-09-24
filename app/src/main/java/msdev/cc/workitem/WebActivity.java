package msdev.cc.workitem;

import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Bitmap;
import android.net.Uri;
import android.os.Bundle;
import android.os.StrictMode;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.webkit.WebResourceRequest;
import android.webkit.WebSettings;
import android.webkit.WebView;
import android.webkit.WebViewClient;
import com.android.volley.*;
import com.android.volley.toolbox.*;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;
import msdev.cc.workitem.models.TokenModel;
import msdev.cc.workitem.services.HttpService;

import java.io.File;
import java.io.IOException;

public class WebActivity extends AppCompatActivity {

    private WebView webView;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        RequestQueue mRequestQueue = MySingleton.getInstance(this.getApplicationContext()).getRequestQueue();
        mRequestQueue.start();

        try {
            this.getAssets().open("test.txt");
        } catch (IOException e) {
            e.printStackTrace();
        }
        super.onCreate(savedInstanceState);

        //判断是否验证登录
        SharedPreferences config = getSharedPreferences("config", 0);
        String refresh_token = config.getString("refresh_token", null);
        if (refresh_token != null) {
            HttpService httpService = new HttpService(this);
            httpService.updateToken(refresh_token);

            //跳转到主页面
            Intent intent = new Intent(WebActivity.this, MainActivity.class);
            startActivity(intent);
            finish();
        }
        setContentView(R.layout.activity_web);
        webView = (WebView) findViewById(R.id.webview);
        WebSettings settings = webView.getSettings();
        settings.setCacheMode(WebSettings.LOAD_DEFAULT);
        settings.setJavaScriptEnabled(true);
        settings.setDefaultTextEncodingName("utf-8");
        webView.setWebViewClient(new MyWebViewClicnt());
        webView.loadUrl("https://workitem.msdev.cc/");
    }

    private class MyWebViewClicnt extends WebViewClient {

        @Override
        public boolean shouldOverrideUrlLoading(WebView view, WebResourceRequest request) {
            view.loadUrl(request.getUrl().toString());
            return true;
        }

        @Override
        public void onPageStarted(WebView view, String url, Bitmap favicon) {
            Uri uri = Uri.parse(url);

            //获取token ,refresh_token
            if (url.toLowerCase().contains("loginresult")) {
                String token = uri.getQueryParameter("token");
                String refresh_token = uri.getQueryParameter("refresh_token");
                String expiration = uri.getQueryParameter("expiration");

                //保存token
                SharedPreferences config = getSharedPreferences("config", 0);
                SharedPreferences.Editor editor = config.edit();
                editor.putString("token", token);
                editor.putString("refresh_token", refresh_token);
                editor.putString("expiration", expiration);
                editor.commit();

                //跳转
                Intent intent = new Intent(WebActivity.this, MainActivity.class);
                startActivity(intent);
                finish();
            }
            super.onPageStarted(view, url, favicon);

        }
    }
}
