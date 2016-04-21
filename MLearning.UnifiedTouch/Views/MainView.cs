using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using ObjCRuntime;
using UIKit;
using Foundation;
using CoreAnimation;
using CoreGraphics;
using CoreImage;
using MLearning.Core.ViewModels;
using Cirrious.MvvmCross.Binding.Touch.Views;
using System;
using System.Collections.Generic;
using MLearning.UnifiedTouch.CustomComponents;

namespace MLearning.UnifiedTouch.Views
{
	[Register("MainView")]
	public class MainView : MvxViewController, IMLODelegate
	{
		UIView searchBackground;
		UIView rightView;
		UIView detailView;
		UILabel circleName;
		UILabel mloName;
		UILabel unidad, unidades;
		UIButton mloOpen;
		UIImageView userImageView;
		UILabel name;
		UILabel lastname;
		LoadingOverlay mlosOverlay, commentsOverlay, loadingView;
		UIScrollView mlosScroll;
		List <MLOItemView> mlosList;
		UIScrollView commentsScroll;
		UIScrollView pendingTable;
		UIScrollView completeQuizzesTable;
		UITextField newCommentText;
		UITextField searchText;
		UITableView circlesTable;

		bool showingRightBar;
		public bool ShowingRightBar {
			get {return showingRightBar;}
			set {showingRightBar = value;}
		}

		bool mloSelected;
		public bool MloSelected {
			get {return mloSelected;}
			set {
				mloSelected = value;
				if (mloSelected) {
					unidades.Hidden = true;
					unidad.Hidden = false;
					mloName.Hidden = false;
					mloOpen.Hidden = false;
				} else {
					unidades.Hidden = false;
					unidad.Hidden = true;
					mloName.Hidden = true;
					mloOpen.Hidden = true;
				}
			}
		}

		int loIndexSelected;
		public int LoIndexSelected {
			get { return loIndexSelected; }
			set { loIndexSelected = value; }
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// ios7 layout
			if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
				EdgesForExtendedLayout = UIRectEdge.None;

			View.BackgroundColor = UIColor.White;

			unidades = new UILabel ();
			unidad  = new UILabel ();
			mloName  = new UILabel ();
			mloOpen  = new UIButton ();
			mlosScroll = new UIScrollView ();
			commentsScroll = new UIScrollView ();
			circleName = new UILabel ();
			pendingTable = new UIScrollView ();
			completeQuizzesTable = new UIScrollView ();
			mlosList = new List<MLOItemView> ();
			mlosOverlay = new LoadingOverlay (new CGRect (400, 170, 500, 100));
			commentsOverlay = new LoadingOverlay (new CGRect (400, 500, 500, 100));

			circlesTable = new UITableView (new CGRect (0, 108, 248, 308));
			detailView = new UIView (new CGRect (330, 0, 670, 768));


			//twice its size to avoid white background when panning fast
			rightView = new UIView (new CGRect (684, 0, 340 * 2, 768));

			addLeftBar ();
			addSearchBar ();
			addDetails ();
			addRightBar ();
			ShowingRightBar = false;
			animateRightView (duration: 0);
			MloSelected = false;
			LoIndexSelected = 0;

			//ResignFirstResponder
			var tap = new UITapGestureRecognizer (() => {
				newCommentText.ResignFirstResponder();
				searchText.ResignFirstResponder();
			});
			tap.CancelsTouchesInView = false;
			View.AddGestureRecognizer (tap);

		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			if (loadingView != null)
				loadingView.Hide ();
			NavigationController.SetNavigationBarHidden (true, true);
		}
			
		#region LeftBar
		private void addLeftBar ()
		{
			CAGradientLayer leftLayer = new CAGradientLayer ()
			{
				Frame = new CGRect (0, 0, 82, 768),
				Colors = Constants.MainViewGradientColors
			};
			View.Layer.InsertSublayer (leftLayer, 0);

			//red logo
			Add (Constants.makeImageView (new CGRect (16, 20, 50, 50), "iOS Resources/muro/logo.png"));

			var backButton = new UIButton (UIButtonType.Custom) {
				Frame = new CGRect (16,90,50,50)
			};
			backButton.SetBackgroundImage (UIImage.FromFile ("iOS Resources/muro/btn_back.png"), UIControlState.Normal);
			Add (backButton);

			var reloadButton = new UIButton (UIButtonType.Custom) {
				Frame = new CGRect (16,160,50,50)
			};
			reloadButton.SetBackgroundImage (UIImage.FromFile ("iOS Resources/muro/btn_reload.png"), UIControlState.Normal);
			Add (reloadButton);

			loadUserInformation ();


			//binding button actions
			var set = this.CreateBindingSet <MainView, MainViewModel> ();
			set.Bind (backButton).To (vm => vm.BackToCirclePostsCommand); 
			set.Bind (reloadButton).To (vm => vm.RefreshCommentsCommand);
			set.Apply ();
		}

		private void loadUserInformation()
		{
			var vm = this.ViewModel as MainViewModel;

			userImageView = new UIImageView (new CGRect (16, 671, 50, 50));
			userImageView.Layer.CornerRadius = 25f;
			userImageView.Layer.MasksToBounds = true;
			Add (userImageView);

			name = Constants.makeLabel (new CGRect (16, 732, 50, 12), UIColor.White, UITextAlignment.Center, Font.Regular, 12);
			name.AdjustsFontSizeToFitWidth = true;
			Add (name);

			lastname = Constants.makeLabel (new CGRect (16, 744, 50, 12), UIColor.White, UITextAlignment.Center, Font.Regular, 12);
			lastname.AdjustsFontSizeToFitWidth = true;
			Add (lastname);

			try
			{
				if(vm.UserImage != null)
					userImageView.Image = UIImage.LoadFromData (NSData.FromArray (vm.UserImage));
				name.Text = vm.UserFirstName;
				lastname.Text = vm.UserLastName;
				vm.PropertyChanged += vm_PropertyChanged;
			}
			catch (Exception e) {
				Console.WriteLine (e.ToString ());
			}
		}
		#endregion

		#region SearchBar
		private void addSearchBar()
		{
			searchBackground = new UIView (new CGRect (82, 0, 248, 768));
			searchBackground.BackgroundColor = Constants.SearchBarColor;

			var searchImage = new UIImageView (new CGRect (24, 18, 192, 42));
			searchImage.Image = UIImage.FromFile ("iOS Resources/muro/searchback.png");
			searchBackground.Add (searchImage);

			searchText = Constants.makeTextField (new CGRect (56, 24, 160, 30), UIColor.DarkGray, "Buscar", Font.Regular, 14);

			searchText.EditingChanged += (sender, e) => 
			{
				Console.WriteLine (searchText.Text);
				var vm = this.ViewModel as MainViewModel;
				vm.CircleFilterString = searchText.Text;
			};
			searchBackground.Add (searchText);

			var circulos = Constants.makeLabel (new CGRect (24, 80, 220, 24), UIColor.Gray, UITextAlignment.Left, Font.Bold, 15);
			circulos.AdjustsFontSizeToFitWidth = true;
			circulos.Text = "CIRCULOS";
			searchBackground.Add(circulos);

			var personas = Constants.makeLabel (new CGRect (24, 424, 220, 24), UIColor.Gray, UITextAlignment.Left, Font.Bold, 15);
			personas.AdjustsFontSizeToFitWidth = true;
			personas.Text = "PERSONAS";
			searchBackground.Add (personas);

			Add (searchBackground);

			loadCircles ();
			loadPeople ();

		}

		private void loadCircles()
		{
			var source = new MvxSimpleTableViewSource (circlesTable, CircleCell.Key, CircleCell.Key);
			circlesTable.RowHeight = 50;
			circlesTable.SeparatorColor = Constants.SearchBarColor;
			circlesTable.Bounces = false;
			circlesTable.Source = source;
			circlesTable.Delegate = new CirclesTableViewDelegate (this);
			circlesTable.BackgroundColor = UIColor.Clear;

			var set = this.CreateBindingSet <MainView, MainViewModel> ();
			set.Bind (source).To (vm => vm.CirclesList);
			set.Apply();

			searchBackground.Add (circlesTable);
			circlesTable.ReloadData ();

		}
			
		public void loadPeople()
		{
			var peopleTable = new UITableView (new CGRect (0, 454, 248, 302));
			var source = new MvxSimpleTableViewSource (peopleTable, PersonCell.Key, PersonCell.Key);
			peopleTable.RowHeight = 50;
			peopleTable.SeparatorColor =Constants.SearchBarColor;
			peopleTable.Bounces = false;
			peopleTable.Source = source;
			peopleTable.BackgroundColor = UIColor.Clear;


			var set = this.CreateBindingSet <MainView, MainViewModel> ();
			set.Bind (source).To (vm => vm.UsersList);
			set.Apply();

			searchBackground.Add (peopleTable);
			peopleTable.ReloadData ();
		}
		#endregion

		#region Details
		private void addDetails ()
		{

			//linea1
			detailView.Add (Constants.makeImageView (new CGRect (30, 69, 610, 1), "iOS Resources/muro/tareas/linea.png"));
			//linea2
			detailView.Add (Constants.makeImageView (new CGRect (30, 316, 610, 1), "iOS Resources/muro/tareas/linea.png"));

			circleName = Constants.makeLabel (new CGRect (48, 28, 483, 30), UIColor.Black, UITextAlignment.Left, Font.Light, 26);
			circleName.AdjustsFontSizeToFitWidth = true;
			circleName.Text = "Curso";
			detailView.Add (circleName);

			var comentarios = Constants.makeLabel (new CGRect (48, 284, 140, 30), UIColor.Black, UITextAlignment.Left, Font.Light, 22);
			comentarios.AdjustsFontSizeToFitWidth = true;
			comentarios.Text = "Comentarios";
			detailView.Add (comentarios);

			Add (detailView);

			/*showed when circle is selected*/
			unidades = Constants.makeLabel (new CGRect (48, 82, 140, 30), UIColor.Black, UITextAlignment.Left, Font.Light, 22);
			unidades.AdjustsFontSizeToFitWidth = true;
			unidades.Text = "Unidades";
			detailView.Add (unidades);
			/*****/

			/*showed when mlo is selected*/
			unidad = Constants.makeLabel (new CGRect (48, 82, 110, 30), UIColor.Gray, UITextAlignment.Left, Font.Light, 22);
			unidad.Text = "Unidad  >";
			detailView.Add (unidad);

			mloName = Constants.makeLabel (new CGRect (150, 82, 180, 30), UIColor.Black, UITextAlignment.Left, Font.Light, 22);
			mloName.Lines = 1;
			detailView.Add (mloName);

			mloOpen = new UIButton (UIButtonType.Custom);
			mloOpen.Frame = new CGRect (300, 86, 74, 22);
			mloOpen.SetImage (UIImage.FromFile("iOS Resources/muro/tareas/btn_open.png"), UIControlState.Normal);
			mloOpen.TouchUpInside += openMLO;

			detailView.Add (mloOpen);
			/*****/

			//commentBox
			detailView.Add (Constants.makeImageView (new CGRect (30, 702, 610, 52), "iOS Resources/muro/tareas/cboard.png"));

			newCommentText = Constants.makeTextField (new CGRect (46, 720, 548, 16), UIColor.Gray, "Comment", Font.Regular, 14);
			detailView.Add (newCommentText);

			try
			{
				var vm = ViewModel as MainViewModel;
				updateCircleNameText (vm.CirclesList[0].name);

				//first row selected
				if (circlesTable.NumberOfRowsInSection (0) > 0) 
				{
					NSIndexPath first = NSIndexPath.FromItemSection (0, 0);
					circlesTable.SelectRow (first, false, UITableViewScrollPosition.None);
					circlesTable.Delegate.RowSelected (circlesTable, first);
				}
			}
			catch(Exception e) 
			{
				Console.WriteLine (e.ToString () + "Could not load circle name");
			}

			var commentButton = new UIButton (UIButtonType.Custom);
			commentButton.Frame = new CGRect (600, 712, 32, 32);
			commentButton.SetBackgroundImage (UIImage.FromFile ("iOS Resources/muro/tareas/btn_send.png"), UIControlState.Normal);
			commentButton.TouchUpInside += doComment;
			detailView.Add (commentButton);

		}

		public void updateCircleNameText(string t)
		{
			/*new circle selected, reload comments and MLOs*/
			commentsScroll.RemoveFromSuperview ();
			commentsScroll.Dispose ();
			mlosScroll.RemoveFromSuperview ();
			mlosScroll.Dispose ();
			/*****/

			Add (mlosOverlay);
			Add (commentsOverlay);
			circleName.Text = t;
		}

		public UIImage ToUIImage (byte[] bytes)
		{
			if (bytes == null)
				return null;

			using (var data = NSData.FromArray (bytes)) {
				return UIImage.LoadFromData (data);
			}
		}

		void loadMLOS()
		{
			mlosScroll.RemoveFromSuperview();
			mlosScroll.Dispose ();

			mlosScroll = new UIScrollView (new CGRect (38, 124, 620, 144));
			mlosScroll.UserInteractionEnabled = true;
			mlosList.Clear();
			var vm = ViewModel as MainViewModel;

			float xPos = 0;
			int i = 0;
			foreach (var mlo in vm.LearningOjectsList) 
			{
				var view = new MLOItemView (i++, xPos, mlo.lo.title, mlo.lo.name, mlo.lo.lastname, mlo.lo.like);
				view.Delegate = this;
				xPos += (float) view.Frame.Width + 2;
				mlosList.Add (view);
				/*try
				{
					view.MloImage.Image = UIImage.LoadFromData (NSData.FromArray (mlo.cover_bytes));
				}
				catch(Exception e) 
				{
					Console.WriteLine (e.ToString ());
				}*/

				mlo.PropertyChanged += (sender, e) => 
				{
					//view.MloImage.Image = UIImage.LoadFromData (NSData.FromArray (mlo.cover_bytes));

					if(e.PropertyName == "cover_bytes") {
						//lo.LOImage = ToUIImage ((sender as MainViewModel.lo_by_circle_wrapper).cover_bytes);
						view.MloImage.Image  = ToUIImage ((sender as MainViewModel.lo_by_circle_wrapper).cover_bytes);
					}
					if(e.PropertyName == "background_bytes")
					{  
						//resetCenterView();
						//view.  = ToUIImage ((sender as MainViewModel.lo_by_circle_wrapper).background_bytes);
						int idxx = 0;
					}


				};
				mlosScroll.Add (view);
			}

			mlosScroll.ContentSize = new CGSize (xPos, mlosScroll.Frame.Height);
			mlosScroll.Bounces = false;
			detailView.Add (mlosScroll);
		}

		void loadPosts()
		{
			commentsScroll.RemoveFromSuperview ();
			commentsScroll.Dispose ();

			commentsScroll = new UIScrollView (new CGRect (0, 334, 670, 340));
			commentsScroll.UserInteractionEnabled = true;

			var vm = ViewModel as MainViewModel;
			float totalHeight = 0;

			foreach (var c in vm.PostsList) 
			{
				var view = new CommentItemView (totalHeight, c.post.name + " " + c.post.lastname, c.post.created_at.ToString(),c.post.text);
				totalHeight += view.getHeight ();
				/*try
				{
					view.Image.Image = UIImage.LoadFromData (NSData.FromArray (c.userImage));
				}
				catch(Exception e) 
				{
					Console.WriteLine (e.ToString ());

				}*/

				c.PropertyChanged += (sender, e) => 
				{
					view.Name.Text = c.post.name + " " + c.post.lastname;
					if (c.userImage != null)
						view.Image.Image = UIImage.LoadFromData (NSData.FromArray (c.userImage));
				};
				commentsScroll.Add (view);
			}

			commentsScroll.Bounces = false;
			commentsScroll.BackgroundColor = UIColor.Clear;
			commentsScroll.ContentSize = new CGSize (commentsScroll.Frame.Width, totalHeight + 20);
			detailView.Add (commentsScroll);
		}


		void loadMLOComments()
		{
			commentsScroll.RemoveFromSuperview ();
			commentsScroll.Dispose ();

			commentsScroll = new UIScrollView (new CGRect (0, 334, 670, 340));
			commentsScroll.UserInteractionEnabled = true;

			var vm = ViewModel as MainViewModel;
			float totalHeight = 0;
			foreach (var c in vm.LOCommentsList) 
			{
				var view = new CommentItemView (totalHeight, c.lo_comment.name + " " + c.lo_comment.lastname, c.lo_comment.created_at.ToString(),c.lo_comment.text);
				totalHeight += view.getHeight ();
				try
				{
					view.Image.Image = UIImage.LoadFromData (NSData.FromArray (c.userImage));
				}
				catch(Exception e) 
				{
					Console.WriteLine (e.ToString ());
				}

				c.PropertyChanged += (sender, e) => 
				{
					view.Name.Text = c.lo_comment.name + " " + c.lo_comment.lastname;
					view.Image.Image = UIImage.LoadFromData (NSData.FromArray (c.userImage));
				};
				commentsScroll.Add (view);
			}

			commentsScroll.Bounces = false;
			commentsScroll.BackgroundColor = UIColor.Clear;
			commentsScroll.ContentSize = new CGSize (commentsScroll.Frame.Width, totalHeight + 20);
			detailView.Add (commentsScroll);
		}
		#endregion

		#region ButtonActions
		void doComment (object sender, EventArgs e)
		{
			var vm = ViewModel as MainViewModel;

			if(newCommentText.Text != string.Empty)
			{
				if(!MloSelected)
				{
					vm.NewPost = newCommentText.Text;
					vm.PostCommand.Execute(null);
				}
				else
				{
					vm.NewLOComment = newCommentText.Text;
					vm.CreateLOCommentCommand.Execute(null);
				}
				newCommentText.Text = string.Empty;
			}
		}

		void openMLO (object sender, EventArgs e)
		{
			loadingView = new LoadingOverlay (Constants.ScreenFrame, alpha: 0.75f);
			loadingView.BackgroundColor = UIColor.Black;
			View.BringSubviewToFront (loadingView);
			Add(loadingView);
			var vm = ViewModel as MainViewModel;
			vm.OpenLOCommand.Execute (vm.LearningOjectsList [LoIndexSelected]);
		}

		#endregion

		#region RightBar
		private void addRightBar()
		{
			var tmp = new UIImageView (new CGRect (0,0,rightView.Frame.Width, rightView.Frame.Height));
			tmp.Image = UIImage.FromFile ("iOS Resources/muro/fondo.png");




			CAGradientLayer rightLayer = new CAGradientLayer ();
			rightLayer.Frame = new CGRect (0, 0, rightView.Frame.Width, rightView.Frame.Height);
			rightLayer.Colors = Constants.MainViewGradientColors;
			rightLayer.Opacity = 0.8f;
			//rightView.Add(tmp);


			rightView.Layer.InsertSublayer(tmp.Layer, 0);
			rightView.Layer.InsertSublayer(rightLayer, 1);
			//tmp.Layer.Mask = rightLayer;

			var hideAndShowButton = new UIButton (UIButtonType.Custom);
			hideAndShowButton.Frame = new CGRect (5, 377, 14, 14);
			hideAndShowButton.SetBackgroundImage (UIImage.FromFile ("iOS Resources/muro/tareas/incomplete_icon.png"), UIControlState.Normal);
			hideAndShowButton.UserInteractionEnabled = true;
			hideAndShowButton.TouchUpInside += (sender, e) => { animateRightView(duration: 0.25f); };

			rightView.Add (hideAndShowButton);

			var tareas = Constants.makeLabel (new CGRect (116, 30, 91, 24), UIColor.White, UITextAlignment.Center, Font.Regular, 20);
			tareas.AdjustsFontSizeToFitWidth = true;
			tareas.Text = "TAREAS";
			rightView.Add (tareas);

			//tareaImage
			rightView.Add (Constants.makeImageView (new CGRect (210, 30, 22, 22), "iOS Resources/muro/tareas/btn_send.png"));
			//barraTarea
			rightView.Add (Constants.makeImageView (new CGRect (24, 68, 316, 66),"iOS Resources/muro/tareas/barra_curso.png"));

			var curso = Constants.makeLabel (new CGRect (70, 95, 55, 17), UIColor.White, UITextAlignment.Center, Font.Regular, 14);
			curso.AdjustsFontSizeToFitWidth = true;
			curso.Text = "CURSO";
			rightView.Add (curso);

			var unidad = Constants.makeLabel (new CGRect (196, 95, 54, 17), UIColor.White, UITextAlignment.Center, Font.Regular, 14);
			unidad.AdjustsFontSizeToFitWidth = true;
			unidad.Text = "UNIDAD";
			rightView.Add (unidad);

			//circle1
			rightView.Add (Constants.makeImageView (new CGRect (134, 84, 37, 36), "iOS Resources/muro/tareas/circulo.png"));
			//circle2
			rightView.Add (Constants.makeImageView (new CGRect (260, 84, 37, 36), "iOS Resources/muro/tareas/circulo.png"));

			var unidadNumber = Constants.makeLabel (new CGRect (268, 95, 21, 17), UIColor.White, UITextAlignment.Center, Font.Regular, 14);
			unidadNumber.AdjustsFontSizeToFitWidth = true;
			unidadNumber.Text = "?";
			rightView.Add (unidadNumber);

			var cursoNumber = Constants.makeLabel (new CGRect (142, 95, 21, 17), UIColor.White, UITextAlignment.Center, Font.Regular, 14);
			cursoNumber.AdjustsFontSizeToFitWidth = true;
			cursoNumber.Text = "?";
			rightView.Add (cursoNumber);

			var set = this.CreateBindingSet <MainView, MainViewModel> ();
			set.Bind (unidadNumber).To (vm => vm.CurrentLOIDSelected);
			set.Bind (cursoNumber).To (vm => vm.CircleID);
			set.Apply ();

			//Pendientes
			var pendientes = Constants.makeLabel (new CGRect (42, 157, 102, 18), UIColor.White, UITextAlignment.Center, Font.Bold, 14);
			pendientes.Text = "PENDIENTES";
			rightView.Add (pendientes);
			//linea1
			rightView.Add (Constants.makeImageView (new CGRect (40,187,284,1), "iOS Resources/muro/tareas/linea.png"));

			//Completadas
			var completadas = Constants.makeLabel (new CGRect (42, 565, 109, 17), UIColor.White, UITextAlignment.Center, Font.Bold, 14);
			completadas.Text = "COMPLETADAS";
			rightView.Add (completadas);
			//linea2
			rightView.Add (Constants.makeImageView (new CGRect (40,592,284,1), "iOS Resources/muro/tareas/linea.png"));



			Add (rightView);


			//Pan Gesture
			var panToShow = new UIPanGestureRecognizer (panToShowView);
			panToShow.MinimumNumberOfTouches = 1;
			panToShow.MaximumNumberOfTouches = 1;
			rightView.AddGestureRecognizer (panToShow);
		}

		void panToShowView (UIPanGestureRecognizer p)
		{
			CGPoint translatedPoint = p.TranslationInView (View);

			if (p.State == UIGestureRecognizerState.Changed) 
			{
				ShowingRightBar = (p.View.Frame.X > (Constants.ScreenFrame.Width - Constants.RightViewX / 2) ) ? false : true;
				Console.WriteLine ((Constants.ScreenFrame.Width - Constants.RightViewX / 2) + "   " + (p.View.Frame.X) + "   " + ShowingRightBar);

				if(p.View.Frame.X >= Constants.ScreenFrame.Width - Constants.RightViewX - 24)
					p.View.Center = new CGPoint (p.View.Center.X + translatedPoint.X, p.View.Center.Y);
				p.SetTranslation (new CGPoint (), View);
			}
			if (p.State == UIGestureRecognizerState.Ended) 
			{
				ShowingRightBar = (p.View.Frame.X > (Constants.ScreenFrame.Width - Constants.RightViewX / 2) ) ? false : true;

				animateRightView (0.15f);
			}

		}

		void animateRightView(float duration)
		{
			UIView.Animate (duration: duration,
				delay:0,
				options: UIViewAnimationOptions.BeginFromCurrentState,
				animation: () => {
					var finalFrame = new CGRect (684, 0, 340, 768);
					finalFrame.X = ShowingRightBar ? finalFrame.X : finalFrame.X + Constants.RightViewX;
					rightView.Frame = finalFrame;
				},
				completion: () => {
					ShowingRightBar = !ShowingRightBar;
				});
		}

		void loadPendingQuizzes()
		{
			pendingTable.RemoveFromSuperview();
			pendingTable.Dispose ();

			pendingTable = new UIScrollView (new CGRect (24, 192, 316, 358));
			pendingTable.UserInteractionEnabled = true;

			var vm = ViewModel as MainViewModel;
			int i = 0;
			foreach (var quiz in vm.PendingQuizzesList) 
			{
				var view = new QuizItemView (i++, "iOS Resources/muro/tareas/incomplete_icon.png", quiz.content);
				pendingTable.Add (view);
			}
			pendingTable.ContentSize = new SizeF (316, 52 * i);
			pendingTable.Bounces = false;
			pendingTable.BackgroundColor = UIColor.Clear;
			rightView.Add (pendingTable);
		}

		void loadCompleteQuizzes()
		{
			completeQuizzesTable.RemoveFromSuperview();
			completeQuizzesTable.Dispose ();

			completeQuizzesTable = new UIScrollView (new CGRect (24, 598, 316, 160));
			completeQuizzesTable.UserInteractionEnabled = true;

			var vm = ViewModel as MainViewModel;
			int i = 0;
			foreach (var quiz in vm.CompletedQuizzesList) 
			{
				var view = new QuizItemView (i++, "iOS Resources/muro/tareas/complete_icon.png", quiz.content);
				completeQuizzesTable.Add (view);
			}
			completeQuizzesTable.ContentSize = new SizeF (316, 52 * i);
			completeQuizzesTable.Bounces = false;
			completeQuizzesTable.BackgroundColor = UIColor.Clear;
			rightView.Add (completeQuizzesTable);

		}
		#endregion

		#region OnPropertyChanged
		private void vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			var vm = this.ViewModel as MainViewModel;
			string property = e.PropertyName;
			switch (property)
			{
				case "UserFirstName":
					name.Text = vm.UserFirstName;
					break;
				case "UserLastName":
					lastname.Text = vm.UserLastName;
					break;
				case "UserImage":
					userImageView.Image = UIImage.LoadFromData (NSData.FromArray (vm.UserImage));
					break;
				case "CirclesList":
					loadCircles();
					(ViewModel as MainViewModel).CirclesList.CollectionChanged += reloadCircles;
					break;
				case "UsersList":
					loadPeople();
					(ViewModel as MainViewModel).UsersList.CollectionChanged += reloadPeople;
					break;
				case "PendingQuizzesList":
					loadPendingQuizzes();
					break;
				case "CompletedQuizzesList":
					loadCompleteQuizzes();
					break;
				case "PostsList":
					loadPosts();
					(ViewModel as MainViewModel).PostsList.CollectionChanged += reloadPosts;
					commentsOverlay.Hide ();
					break;
				case "LOCommentsList":
					loadMLOComments ();
					(ViewModel as MainViewModel).LOCommentsList.CollectionChanged += reloadMLOComments;
					commentsOverlay.Hide ();
					break;
				case "LearningOjectsList":
					loadMLOS ();
					(ViewModel as MainViewModel).LearningOjectsList.CollectionChanged += reloadMLOs;
					mlosOverlay.Hide ();
					break;
				default:
					break;
			}
		}

		private void reloadCircles (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			loadCircles ();
		}
		private void reloadPeople (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			loadPeople ();
		}
		private void reloadPosts (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			loadPosts ();
		}
		private void reloadMLOs (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			loadMLOS ();
		}
		private void reloadMLOComments (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			loadMLOComments ();
		}
		#endregion

		#region IMLODelegate
		public void MLOItemLiked (int index)
		{
			var vm = ViewModel as MainViewModel;
			vm.LikeLOCommand.Execute(vm.LearningOjectsList[index]);
		}

		public void MLOItemSelected (int index)
		{
			Add (commentsOverlay);
			var vm = ViewModel as MainViewModel;

			foreach (MLOItemView item in mlosList) 
			{
				if (item.Index != index)
					item.IsSelected = false;
				else 
				{
					vm.SelectLOCommand.Execute (vm.LearningOjectsList [index]);
					mloName.Text = vm.LearningOjectsList [index].lo.title;
					mloName.SizeToFit ();
					mloOpen.Frame = new CGRect (mloName.Frame.X + mloName.Frame.Width + 15, 86, 74, 22);
					Console.WriteLine (mloOpen.Frame);
				}
			}
			MloSelected = true;
			LoIndexSelected = index;
		}
		#endregion

	}
}