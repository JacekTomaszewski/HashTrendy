package com.example.johncena.hashapp;

import android.app.Activity;
import android.content.Context;
import android.telephony.TelephonyManager;
import android.widget.TextView;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.StringRequest;
import com.android.volley.toolbox.Volley;

/**
 * Created by Marcin on 25.04.2017.
 */

public class PushDeviceImei extends Activity {
    private void pushDeviceImei()
    {
        Imei imei = new Imei();

        final TextView textView = (TextView) findViewById(R.id.textView);

        String url ="http://projekthashtag.ddns.net:51577/hash/create?imei=" + imei.deviceId;
        // Instantiate the RequestQueue.
        RequestQueue queue = Volley.newRequestQueue(this);

        // Request a string response from the provided URL.
        StringRequest stringRequest = new StringRequest(Request.Method.POST, url,
                new Response.Listener<String>() {
                    @Override
                    public void onResponse(String response) {
                        // Display the first 500 characters of the response string.
                        textView.setText("Dodano rekord!");
                    }
                }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                textView.setText("Błąd. Nie dodano rekordu!!");
            }
        });
// Add the request to the RequestQueue.
        queue.add(stringRequest);
    }
}
