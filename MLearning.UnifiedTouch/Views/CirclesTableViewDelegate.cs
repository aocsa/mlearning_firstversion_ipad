using System;
using UIKit;
using MLearning.UnifiedTouch.Views;
using MLearning.Core.ViewModels;

namespace MLearning.UnifiedTouch
{
	public class CirclesTableViewDelegate : UITableViewDelegate
	{
		MainView mv;

		public CirclesTableViewDelegate (MainView v)
		{
			mv = v;
		}
		public override void RowSelected (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			var cell = tableView.CellAt (indexPath) as CircleCell;
			mv.updateCircleNameText (cell.CircleName.Text);
			mv.MloSelected = false;
			var vm = mv.ViewModel as MainViewModel;
			vm.SelectCircleCommand.Execute (vm.CirclesList[indexPath.Row]);

		}
	}
}

