package msdev.cc.workitem.Helper;

import java.io.InputStream;
import java.util.Scanner;

/**
 * Created by zpty on 2017/9/25.
 */

public class StringHelper {

    public static String convertStreamToString(InputStream is) {
        Scanner s = new Scanner(is).useDelimiter("\\A");
        return s.hasNext() ? s.next() : "";
    }
}
