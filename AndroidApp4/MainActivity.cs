using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.BottomNavigation;
using Google.Android.Material.Button;
using Google.Android.Material.Chip;
using Google.Android.Material.Dialog;
using Google.Android.Material.MaterialSwitch;
using Google.Android.Material.ProgressIndicator;
using Google.Android.Material.Slider;
using Google.Android.Material.Snackbar;
using Google.Android.Material.Tabs;
using Google.Android.Material.TextField;

namespace AndroidApp4;

[Activity(Label = "@string/app_name", MainLauncher = true)]
public class MainActivity : AppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.activity_main);

        InitializeComponentShowcase();
    }

    private void InitializeComponentShowcase()
    {
        var snackbarButton = FindViewById<MaterialButton>(Resource.Id.button_show_snackbar);
        var dialogButton = FindViewById<MaterialButton>(Resource.Id.button_show_dialog);
        var outlinedButton = FindViewById<MaterialButton>(Resource.Id.button_outlined);
        var actionButton = FindViewById<MaterialButton>(Resource.Id.button_primary);
        var confirmationButton = FindViewById<MaterialButton>(Resource.Id.button_confirmation);
        var nameInput = FindViewById<TextInputEditText>(Resource.Id.input_name);
        var nameInputLayout = FindViewById<TextInputLayout>(Resource.Id.input_name_layout);
        var helperLabel = FindViewById<TextView>(Resource.Id.text_state_summary);
        var materialSwitch = FindViewById<MaterialSwitch>(Resource.Id.switch_dynamic);
        var chipGroup = FindViewById<ChipGroup>(Resource.Id.chip_group);
        var tabs = FindViewById<TabLayout>(Resource.Id.tabs_showcase);
        var bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);
        var slider = FindViewById<Slider>(Resource.Id.slider_progress);
        var linearIndicator = FindViewById<LinearProgressIndicator>(Resource.Id.progress_linear);
        var circularIndicator = FindViewById<CircularProgressIndicator>(Resource.Id.progress_circular);

        if (tabs is not null && tabs.TabCount == 0)
        {
            tabs.AddTab(tabs.NewTab().SetText(Resource.String.tab_overview));
            tabs.AddTab(tabs.NewTab().SetText(Resource.String.tab_inputs));
            tabs.AddTab(tabs.NewTab().SetText(Resource.String.tab_feedback));
        }

        if (slider is not null && linearIndicator is not null)
        {
            slider.Change += (_, args) =>
            {
                var value = args.P1;
                linearIndicator.Progress = (int)value;
                if (helperLabel is not null)
                {
                    helperLabel.Text = GetString(Resource.String.state_summary_format, nameInput?.Text ?? string.Empty, (int)value);
                }
            };
        }

        if (materialSwitch is not null && circularIndicator is not null)
        {
            materialSwitch.CheckedChange += (_, args) =>
            {
                circularIndicator.Visibility = args.IsChecked ? Android.Views.ViewStates.Visible : Android.Views.ViewStates.Gone;
            };
        }

        if (chipGroup is not null)
        {
            chipGroup.CheckedStateChange += (_, args) =>
            {
                if (helperLabel is null)
                {
                    return;
                }

                // CheckedIds is not exposed in this binding version; count checked chips directly.
                var checkedCount = 0;
                for (var i = 0; i < chipGroup.ChildCount; i++)
                {
                    if (chipGroup.GetChildAt(i) is Chip chip && chip.Checked)
                        checkedCount++;
                }
                helperLabel.Text = GetString(Resource.String.state_summary_selection_format, checkedCount);
            };
        }

        if (bottomNavigation is not null)
        {
            bottomNavigation.ItemSelected += (_, args) =>
            {
                if (helperLabel is not null)
                {
                    helperLabel.Text = args.Item.TitleFormatted?.ToString() ?? GetString(Resource.String.showcase_ready);
                }
            };
        }

        if (snackbarButton is not null)
        {
            snackbarButton.Click += (_, _) =>
            {
                Snackbar
                    .Make(snackbarButton, Resource.String.snackbar_message, Snackbar.LengthLong)
                    .SetAction(Resource.String.snackbar_action, _ =>
                    {
                        if (helperLabel is not null)
                        {
                            helperLabel.Text = GetString(Resource.String.snackbar_action_feedback);
                        }
                    })
                    .Show();
            };
        }

        if (dialogButton is not null)
        {
            dialogButton.Click += (_, _) =>
            {
                new MaterialAlertDialogBuilder(this)
                    .SetTitle(Resource.String.dialog_title)
                    .SetMessage(Resource.String.dialog_message)
                    .SetPositiveButton(Resource.String.dialog_positive, (_, _) =>
                    {
                        if (helperLabel is not null)
                        {
                            helperLabel.Text = GetString(Resource.String.dialog_confirmed);
                        }
                    })
                    .SetNegativeButton(Resource.String.dialog_negative, (_, _) => { })
                    .Show();
            };
        }

        if (actionButton is not null)
        {
            actionButton.Click += (_, _) =>
            {
                if (helperLabel is not null)
                {
                    helperLabel.Text = GetString(Resource.String.state_summary_format, nameInput?.Text ?? string.Empty, (int)(slider?.Value ?? 0));
                }
            };
        }

        if (confirmationButton is not null && nameInput is not null && nameInputLayout is not null)
        {
            confirmationButton.Click += (_, _) =>
            {
                var inputName = nameInput.Text?.Trim();

                if (string.IsNullOrWhiteSpace(inputName))
                {
                    nameInputLayout.Error = GetString(Resource.String.input_name_error_required);
                    nameInput.RequestFocus();
                    return;
                }

                nameInputLayout.Error = null;
                nameInputLayout.ErrorEnabled = false;
            };
        }

        if (outlinedButton is not null && nameInput is not null)
        {
            outlinedButton.Click += (_, _) => nameInput.Text = string.Empty;
        }

        var radioDark = FindViewById<RadioButton>(Resource.Id.radio_dark);
        if (radioDark is not null)
        {
            radioDark.CheckedChange += (_, args) =>
            {
                AppCompatDelegate.DefaultNightMode =
                    args.IsChecked ? AppCompatDelegate.ModeNightYes : AppCompatDelegate.ModeNightNo;
            };
        }
    }
}