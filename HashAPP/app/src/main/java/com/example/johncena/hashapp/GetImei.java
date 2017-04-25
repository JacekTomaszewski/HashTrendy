package com.example.johncena.hashapp;

import android.Manifest;
import android.app.Activity;
import android.content.pm.PackageManager;
import android.os.Bundle;
import android.telephony.TelephonyManager;
import android.widget.TextView;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.StringRequest;
import com.android.volley.toolbox.Volley;

/**
 * Created by JohnCena on 4/18/2017.
 */

public class GetImei extends Activity
{
    private static final int PERMISSIONS_REQUEST_READ_PHONE_STATE = 999;
    private TelephonyManager telephonyManager;
    private String deviceUniqueId;

    public void onCreate(Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        if (checkSelfPermission(Manifest.permission.READ_PHONE_STATE)
                != PackageManager.PERMISSION_GRANTED) {
            requestPermissions(new String[]{Manifest.permission.READ_PHONE_STATE},
                    PERMISSIONS_REQUEST_READ_PHONE_STATE);
        } else {
            getDeviceImei();
            pushDeviceImei();
        }
    }


    private void getDeviceImei()
    {
                        deviceUniqueId = "EmulatorImei";
        TextView textViewImei = (TextView) findViewById(R.id.textViewImei);
        textViewImei.setText(deviceUniqueId);
        // !!! IF you want to run on mobile device !!!
        //  telephonyManager = (TelephonyManager) getSystemService(Context.TELEPHONY_SERVICE);
        //  deviceId = telephonyManager.getDeviceId().toString();
    }

    private void pushDeviceImei() {


        final TextView textViewResult = (TextView) findViewById(R.id.textViewResult);

        String url = "http://projekthashtag.ddns.net:51577/hash/create?imei=" + deviceUniqueId;
        RequestQueue queue = Volley.newRequestQueue(this);
        StringRequest stringRequest = new StringRequest(Request.Method.POST, url,
                new Response.Listener<String>() {
                    @Override
                    public void onResponse(String response) {
                        textViewResult.setText("Twój imei został dodany lub jest już w bazie");
                    }
                }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                textViewResult.setText("Błąd. Nie dodano rekordu!!");
            }
        });
        queue.add(stringRequest);
    }
}
