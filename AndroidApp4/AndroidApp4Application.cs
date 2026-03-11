using Android.App;
using Android.Runtime;
using Google.Android.Material.Color;

namespace AndroidApp4;

[Application]
public class AndroidApp4Application : Application
{
    public AndroidApp4Application(nint handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    public override void OnCreate()
    {
        base.OnCreate();
        DynamicColors.ApplyToActivitiesIfAvailable(this);
    }
}