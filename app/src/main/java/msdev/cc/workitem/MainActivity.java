package msdev.cc.workitem;

import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.design.widget.BottomNavigationView;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentTransaction;
import android.support.v7.app.AppCompatActivity;
import android.view.MenuItem;
import android.widget.TextView;

public class MainActivity extends AppCompatActivity implements HomeFragment.OnFragmentInteractionListener {

    private TextView mTextMessage;
    protected FragmentManager fragmentManager;

    private BottomNavigationView.OnNavigationItemSelectedListener mOnNavigationItemSelectedListener
            = new BottomNavigationView.OnNavigationItemSelectedListener() {

        @Override
        public boolean onNavigationItemSelected(@NonNull MenuItem item) {
            setFragment(item.getItemId());
            return false;
        }

    };


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        mTextMessage = (TextView) findViewById(R.id.message);

        BottomNavigationView navigation = (BottomNavigationView) findViewById(R.id.navigation);
        navigation.setOnNavigationItemSelectedListener(mOnNavigationItemSelectedListener);


        Intent intent = getIntent();
        String token = intent.getStringExtra("token");
        String expiration = intent.getStringExtra("expiration");

        mTextMessage.setText(token);

    }


    protected void setFragment(int itemId) {
        fragmentManager = this.getSupportFragmentManager();
        FragmentTransaction transaction = fragmentManager.beginTransaction();
        switch (itemId) {
            case R.id.navigation_home:
                HomeFragment fragment = new HomeFragment();
                transaction.replace(R.id.main_frame, fragment);
                break;
            case R.id.navigation_dashboard:
                mTextMessage.setText(R.string.title_dashboard);
                break;
            case R.id.navigation_notifications:
                mTextMessage.setText(R.string.title_notifications);
                break;
        }
        transaction.commit();

    }

    @Override
    public void onFragmentInteraction(Uri uri) {

    }

}
