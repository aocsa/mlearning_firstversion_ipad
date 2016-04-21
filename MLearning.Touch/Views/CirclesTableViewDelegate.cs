using System;
using MonoTouch.UIKit;
using MLearning.Touch.Views;
using MLearning.Core.ViewModels;

namespace MLearning.Touch
{
	public class CirclesTableViewDelegate : UITableViewDelegate
	{
		MainView mv;

		public CirclesTableViewDelegate (MainView v)
		{
			mv = v;
		}
		public override void RowSelected (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			var cell = tableView.CellAt (indexPath) as CircleCell;
			mv.updateCircleNameText (cell.CircleName.Text);
			mv.MloSelected = false;
			var vm = mv.ViewModel as MainViewModel;
			vm.SelectCircleCommand.Execute (vm.CirclesList[indexPath.Row]);

		}
	}
}

