using CruzeMob.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CruzeMob
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            
            imgTargetPhoto.Clicked += async (sender, args) =>
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("No Camera", ":( No Camera Available", "OK");
                    return;
                }
                imgTargetPhoto.Source = ImageSource.FromResource("CruzeMob.Images.Scan.png", typeof(MainPage).GetTypeInfo().Assembly);
                lblStatus.Text = "Tap to Scan";

                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    Directory = "Test",
                    SaveToAlbum = true,
                    CompressionQuality = 75,
                    PhotoSize = PhotoSize.MaxWidthHeight,
                    MaxWidthHeight = 1000,
                    DefaultCamera = CameraDevice.Rear
                });

                if (file == null)
                    return;

                imgTargetPhoto.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
                lblStatus.Text = "Verifying..." +
                "";
                string base64PhotoTarget = await ToBase64(file);
                file.Dispose();                

                IdentifyRequest identifyRequest = new IdentifyRequest()
                {
                    CarrierCode = "AS",
                    FlightNumber = "1275",
                    ScheduledEncounterPort = "LAS",
                    ScheduledEncounterDate = "20200716",
                    PhotoDate="20200716",
                    DeviceId="Device1",
                    DepartureTerminal="3",
                    DepartureGate="E8",
                    Photo = base64PhotoTarget,
                    Token="MyToken"
                };
                IdentifyResponse identifyResponse = await App.CruzeApi.IdentifyAsync(identifyRequest);
                if (!String.IsNullOrEmpty(identifyResponse.Result))
                {
                    if (identifyResponse.Result.Equals("Match"))
                    {
                        imgTargetPhoto.Source = ImageSource.FromResource("CruzeMob.Images.Pass.png", typeof(MainPage).GetTypeInfo().Assembly);
                        lblStatus.Text = string.Format(@"{0}: UID = [{1}]", identifyResponse.Result, identifyResponse.UID);
                    }
                    else
                    {
                        imgTargetPhoto.Source = ImageSource.FromResource("CruzeMob.Images.Fail.png", typeof(MainPage).GetTypeInfo().Assembly);
                        lblStatus.Text = string.Format(@"{0}", identifyResponse.Result);
                    }
                }
                else
                {
                    lblStatus.Text = "No Face Detected or Poor Image";
                }
            };

            async Task<string> ToBase64(MediaFile file)
            {
                var stream = file.GetStream();
                var bytes = new byte[stream.Length];
                await stream.ReadAsync(bytes, 0, (int)stream.Length);
                string base64 = System.Convert.ToBase64String(bytes);
                file.Dispose();
                return base64;
            }
        }
    }
}
