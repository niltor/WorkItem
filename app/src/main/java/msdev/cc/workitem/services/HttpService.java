package msdev.cc.workitem.services;

import android.content.Context;
import android.content.SharedPreferences;
import com.android.volley.*;
import com.android.volley.toolbox.BasicNetwork;
import com.android.volley.toolbox.DiskBasedCache;
import com.android.volley.toolbox.HurlStack;
import com.android.volley.toolbox.StringRequest;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;
import msdev.cc.workitem.Helper.StringHelper;
import msdev.cc.workitem.MySingleton;
import msdev.cc.workitem.models.TokenModel;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.URLEncoder;
import java.util.HashMap;
import java.util.Map;


/**
 * Created by NilTor on 2017/9/22.
 */

public class HttpService {


    private Context _context;

    public HttpService(Context context) {
        _context = context;
        RequestQueue mRequestQueue;
        Cache cache = new DiskBasedCache(_context.getCacheDir(), 1024 * 1024); // 1MB cap
        Network network = new BasicNetwork(new HurlStack());
        mRequestQueue = new RequestQueue(cache, network);
        mRequestQueue.start();
    }

    public void updateToken(final String refreshToken) {
        String url = "https://app.vssps.visualstudio.com/oauth2/token";
        try {
            InputStream inputStream = _context.getAssets().open("appsettings.Production.json");
            String settings = StringHelper.convertStreamToString(inputStream);

            JSONObject object = new JSONObject(settings);

            final String ClientSecret = object.getString("ClientSecrect");
            final String CallbackUrl = object.getString("CallbackUrl");

            StringRequest stringRequest =
                new StringRequest(Request.Method.POST,
                    url,
                    new Response.Listener<String>() {
                        @Override
                        public void onResponse(String response) {
                            Gson gson = new Gson();
                            TokenModel tokenModel = gson.fromJson(response, new TypeToken<TokenModel>() {
                            }.getType());

                            SharedPreferences config = _context.getSharedPreferences("config", 0);
                            SharedPreferences.Editor editor = config.edit();
                            editor.putString("token", tokenModel.access_token);
                            editor.putString("refresh_token", tokenModel.refresh_token);
                            editor.putString("expiration", tokenModel.expires_in);
                            editor.commit();
                        }
                    },
                    new Response.ErrorListener() {
                        @Override
                        public void onErrorResponse(VolleyError error) {
                        }
                    }
                ) {
                    @Override
                    public Map<String, String> getHeaders() throws AuthFailureError {
                        Map<String, String> header = new HashMap<>();
                        header.put("Content-Type", "application/x-www-form-urlencoded");
                        header.put("Content-Length", getBody().length + "");
                        return header;
                    }

                    @Override
                    public byte[] getBody() throws AuthFailureError {
                        return GenerateRequestPostData(ClientSecret, refreshToken, CallbackUrl).getBytes();
                    }
                };
            MySingleton.getInstance(_context).addToRequestQueue(stringRequest);
        } catch (IOException e) {
            e.printStackTrace();
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    private String GenerateRequestPostData(String clientSecret, String refreshToken, String callbackUrl) {
        return String.format("client_assertion_type=urn:ietf:params:oauth:client-assertion-type:jwt-bearer&client_assertion=%s&grant_type=refresh_token&assertion=%s&redirect_uri=%s",
            URLEncoder.encode(clientSecret),
            URLEncoder.encode(refreshToken),
            callbackUrl
        );
    }
}
