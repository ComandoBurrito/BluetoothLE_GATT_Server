using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Bluetooth_GATTServer
{
    /// <summary>
    /// Fournit un comportement sp�cifique � l'application afin de compl�ter la classe Application par d�faut.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initialise l'objet d'application de singleton.  Il s'agit de la premi�re ligne du code cr��
        /// � �tre ex�cut�e. Elle correspond donc � l'�quivalent logique de main() ou WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoqu� lorsque l'application est lanc�e normalement par l'utilisateur final.  D'autres points d'entr�e
        /// seront utilis�s par exemple au moment du lancement de l'application pour l'ouverture d'un fichier sp�cifique.
        /// </summary>
        /// <param name="e">D�tails concernant la requ�te et le processus de lancement.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Ne r�p�tez pas l'initialisation de l'application lorsque la fen�tre comporte d�j� du contenu,
            // assurez-vous juste que la fen�tre est active
            if (rootFrame == null)
            {
                // Cr�ez un Frame utilisable comme contexte de navigation et naviguez jusqu'� la premi�re page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: chargez l'�tat de l'application pr�c�demment suspendue
                }

                // Placez le frame dans la fen�tre active
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // Quand la pile de navigation n'est pas restaur�e, acc�dez � la premi�re page,
                    // puis configurez la nouvelle page en transmettant les informations requises en tant que
                    // param�tre
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // V�rifiez que la fen�tre actuelle est active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Appel� lorsque la navigation vers une page donn�e �choue
        /// </summary>
        /// <param name="sender">Frame � l'origine de l'�chec de navigation.</param>
        /// <param name="e">D�tails relatifs � l'�chec de navigation</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Appel� lorsque l'ex�cution de l'application est suspendue.  L'�tat de l'application est enregistr�
        /// sans savoir si l'application pourra se fermer ou reprendre sans endommager
        /// le contenu de la m�moire.
        /// </summary>
        /// <param name="sender">Source de la requ�te de suspension.</param>
        /// <param name="e">D�tails de la requ�te de suspension.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: enregistrez l'�tat de l'application et arr�tez toute activit� en arri�re-plan
            deferral.Complete();
        }
    }
}