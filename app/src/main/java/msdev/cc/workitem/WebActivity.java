package msdev.cc.workitem;

import android.content.Intent;
import android.graphics.Bitmap;
import android.net.Uri;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.webkit.WebResourceRequest;
import android.webkit.WebSettings;
import android.webkit.WebView;
import android.webkit.WebViewClient;

public class WebActivity extends AppCompatActivity {

    private WebView webView;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
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
            Log.i("msdev", url);
            if (url.toLowerCase().contains("loginresult")) {
                String token = uri.getQueryParameter("token");
                String expiration = uri.getQueryParameter("expiration");
                Intent intent = new Intent(WebActivity.this, MainActivity.class);
                intent.putExtra("token", token);
                intent.putExtra("expiration", expiration);
                startActivity(intent);
            }
            super.onPageStarted(view, url, favicon);

        }
    }
}
