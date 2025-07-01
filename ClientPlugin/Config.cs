using BreathControl.Settings;
using BreathControl.Settings.Elements;
using Sandbox.Graphics.GUI;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using VRage.Input;
using VRageMath;
using Sandbox.Game.Entities.Character;

namespace BreathControl
{

    public class Config : INotifyPropertyChanged
    {
        #region Options

        // TODO: Define your configuration options and their default values
        private bool BreathToggleVal = true;
        private float BreathVolumeMultVal = 0.1f;

        #endregion

        #region User interface

        // TODO: Settings dialog title
        public readonly string Title = "Breath Control Options";

        // TODO: Settings dialog controls, one property for each configuration option

        [Checkbox(label: "Heavy Breathing", description: "Enable or disable the character's heavy breathing noises.")]
        public bool BreathToggle
        {
            get => BreathToggleVal;
            set => SetField(ref BreathToggleVal, value);
        }

        [Slider(0f, 2f, 0.5f, SliderAttribute.SliderType.Float, label:"Breathing Volume" , description: "Set a multiplier for the character's heavy breathing volume.")]
        public float BreathVolumeMult
        {
            get => BreathVolumeMultVal;
            set => SetField(ref BreathVolumeMultVal, value);
        }

        #endregion

        #region Property change notification bilerplate

        public static readonly Config Default = new Config();
        public static readonly Config Current = ConfigStorage.Load();

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion
    }
}