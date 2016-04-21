using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MLearning.Touch.CustomComponents;
using System;
using MonoTouch.CoreAnimation;
using MonoTouch.CoreGraphics;
using Core.ViewModels;

namespace MLearning.Touch.Views
{
	[Register("LoginView")]
	public class LoginView : MvxViewController
	{
		UILabel _text_lu;
		public UILabel Text_lu 
		{
			get {return _text_lu;}
			set {_text_lu = value;}
		}

		UILabel _text_ru;
		public UILabel Text_ru {
			get {return _text_ru;}
			set {_text_ru = value;}
		}

		UILabel _text_rb;
		public UILabel Text_rb {
			get {return _text_rb;}
			set {_text_rb = value;}
		}

		UIView form;
		public UIView Form {
			get {return form;}
			set {form = value;}
		}

		GradientComponent gradient;
		UILabel login;
		UILabel register;
		UIButton loginButton;
		UITextField username;
		UITextField password;
		LoadingOverlay loadingView;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			View.BackgroundColor = UIColor.White;

			// ios7 layout
			if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
				EdgesForExtendedLayout = UIRectEdge.None;
				

			gradient = new GradientComponent (frame: Constants.ScreenFrame, animationDuration: 15);
			View.Layer.InsertSublayer (gradient, 0);

			loadLogo ();
			//logo
			//Add (getResource (new RectangleF (20, 715, 34, 35), "iOS Resources/bigLogo.png"));

			//grilla
			Add (Constants.makeImageView (Constants.ScreenFrame, "iOS Resources/grilla.png"));

			loadText ();

		}
			
		private void loadLogo()
		{
			var bigLogo = Constants.makeImageView (new RectangleF (408, 280, 208, 208), "iOS Resources/bigLogo.png");
			Add (bigLogo);
			UIView.Animate (duration: 1,
				delay: 0,
				options: UIViewAnimationOptions.CurveEaseIn,
				animation: () => {
					bigLogo.Frame = new RectangleF (20,715,34,35);
				},
				completion: () => {
					loadForm();
					bigLogo.Hidden = true;
					bigLogo.RemoveFromSuperview();
					bigLogo.Dispose ();
					Add (Constants.makeImageView (new RectangleF (20, 715, 34, 35), "iOS Resources/logo.png"));

				});
		}

		private void loadForm()
		{
			//formulario background
			Form = Constants.makeImageView (new RectangleF (379, 251, 266, 267), "iOS Resources/formulario.png");
			//separator 1
			Form.Add (Constants.makeImageView (new RectangleF (398 - Form.Frame.X , 410 - Form.Frame.Y , 228, 1) , "iOS Resources/separator.png"));
			//separator 2
			Form.Add (Constants.makeImageView (new RectangleF (398 - Form.Frame.X , 457 - Form.Frame.Y , 228, 1) , "iOS Resources/separator.png"));

			login = Constants.makeLabel (new RectangleF (444 - Form.Frame.X, 290 - Form.Frame.Y, 130, 53),
										 Constants.LoginLabelColor, UITextAlignment.Center, Font.Light, 22); 

			login.Text = "Iniciar sesión en Aplicación";
			login.LineBreakMode = UILineBreakMode.WordWrap;
			login.Lines = 0;
			login.AdjustsFontSizeToFitWidth = true;
			Form.Add (login);

			register = Constants.makeLabel (new RectangleF (529 - Form.Frame.X, 476 - Form.Frame.Y, 77, 17),
											Constants.LoginLabelColor, UITextAlignment.Center, Font.Light, 16);
			register.Text = "Registrarse";
			register.AdjustsFontSizeToFitWidth = true;
			Form.Add (register);


			loginButton = new UIButton (UIButtonType.Custom);
			loginButton.Frame = new RectangleF (604 - Form.Frame.X, 428 - Form.Frame.Y, 22, 23);
			loginButton.SetBackgroundImage (UIImage.FromFile ("iOS Resources/boton.png"), UIControlState.Normal);
			Form.Add (loginButton);

			username = Constants.makeTextField (new RectangleF (409 - Form.Frame.X, 388 - Form.Frame.Y, 180, 15), 
				Constants.LoginTextColor, "Usuario", Font.Light, 16);
			username.BackgroundColor = UIColor.Clear;
			Form.Add (username);


			password = Constants.makeTextField (new RectangleF (407 - Form.Frame.X, 434 - Form.Frame.Y, 180, 15),
				Constants.LoginTextColor, "Contraseña", Font.Light, 16);
			password.BackgroundColor = UIColor.Clear;
			password.SecureTextEntry = true;
			Form.Add (password);


			//Form control when keyboard appears
			username.EditingDidBegin += (sender, e) => 	{ beginEditing(true); };
			password.EditingDidBegin += (sender, e) => 	{ beginEditing(true); };
			username.EditingDidEnd += (sender, e) => 	{ beginEditing(false);};
			password.EditingDidEnd += (sender, e) =>	{ beginEditing(false);};

			Form.UserInteractionEnabled = true;
			username.UserInteractionEnabled = true;
			Form.Alpha = 0;

			Add (Form);
			UIView.Animate (duration: 0.75,
				animation: () => {
					Form.Alpha = 1;
				});


			createBindings ();

			//hide keyboard
			var tap = new UITapGestureRecognizer (() => { username.ResignFirstResponder(); password.ResignFirstResponder();} );
			View.AddGestureRecognizer (tap);
		}

		private void beginEditing (bool editing)
		{
			float duration = 0.3f;

			int move = (editing ? Constants.LoginFormOffsetY : Constants.LoginFormOffsetY * -1); 

			RectangleF currentFrame = Form.Frame;
			RectangleF newFrame = new RectangleF (currentFrame.X, currentFrame.Y + move, currentFrame.Width, currentFrame.Height);

			UIView.Animate (duration: duration,
				animation: () => { Form.Frame = newFrame; });
		}

		private void createBindings()
		{
			/*Connection to ViewModel*/
			var set = this.CreateBindingSet<LoginView, LoginViewModel> ();
			set.Bind (username).To (vm => vm.Username);
			set.Bind (password).To (vm => vm.Password);
			set.Apply ();

			loginButton.TouchUpInside += (sender, e) => 
			{
				var command = ((LoginViewModel)this.ViewModel).LoginCommand;
				command.Execute(null);
				loadingView = new LoadingOverlay (Constants.ScreenFrame, alpha: 0.75f);
				Add(loadingView);
			};


			((LoginViewModel) ViewModel).PropertyChanged += (sender, e) => 
			{
				var vm = (LoginViewModel)this.ViewModel;
				if (e.PropertyName == "LoginOK")
				{
					if (!vm.LoginOK)
					{
						alertMessage ("Ingrese datos correctos");
						loadingView.Hide();
					}  
				}

				if (e.PropertyName == "ConnectionOK")
				{
					if (!vm.ConnectionOK)
					{
						alertMessage ("Verifique su conexión de Internet");
						loadingView.Hide();
					}  
				}
			};
		}

		private void loadText()
		{
			Text_lu = Constants.makeLabel (new RectangleF (17, 20, 80, 16), UIColor.White, UITextAlignment.Left, Font.Regular, 16);
			Text_lu.Text = "Aplicación";

			Text_ru = Constants.makeLabel (new RectangleF (918, 20, 88, 13), UIColor.White, UITextAlignment.Right, Font.Regular, 16);
			Text_ru.Text = "Learn More";

			Text_rb = Constants.makeLabel (new RectangleF (491, 738, 506, 16), UIColor.White, UITextAlignment.Right, Font.Regular, 14);
			Text_rb.Text = "Forgot your Apple ID or password | Privacy policy | Copyrigth 2014";

			Add (Text_lu);
			Add (Text_ru);
			Add (Text_rb);
		}

		private void alertMessage (string message)
		{
			var alert = new UIAlertView ("Error", message, null, "OK", null);
			alert.Show ();
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			NavigationController.SetNavigationBarHidden (true, true);
		}

	}
}