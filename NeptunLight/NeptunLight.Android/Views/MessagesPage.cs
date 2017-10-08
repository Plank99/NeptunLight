﻿using System;
using System.Reactive.Linq;
using System.Windows.Input;
using Android.App;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using NeptunLight.Droid.Utils;
using NeptunLight.ViewModels;
using ReactiveUI;

namespace NeptunLight.Droid.Views
{
    public class MessagesPage : ReactiveFragment<MessagesPageViewModel>, SwipeRefreshLayout.IOnRefreshListener
    {
        private LayoutInflater _layoutInflater;
        public ListView MessageList { get; set; }

        public SwipeRefreshLayout SwipeRefresh { get; set; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _layoutInflater = inflater;
            View layout = inflater.Inflate(Resource.Layout.MessagesPage, container, false);

            this.WireUpControls(layout);

            SwipeRefresh.SetOnRefreshListener(this);

            return layout;
        }

        public override void OnStart()
        {
            base.OnStart();


            this.WhenAnyValue(x => x.ViewModel.Messages).Subscribe(items =>
            {
                var messageListAdapter = new ListAdapter<MessageViewModel>(_layoutInflater, items, Resource.Layout.MessageListItem, (itemView, model) =>
                {
                    itemView.FindViewById<TextView>(Resource.Id.titleTextView).Text = model.Subject;
                    itemView.FindViewById<TextView>(Resource.Id.senderTextView).Text = model.Sender;
                    itemView.FindViewById<TextView>(Resource.Id.letterBox).Text = model.SenderCode;
                });
                MessageList.Adapter = messageListAdapter;
                MessageList.Events().ItemClick.Select(args => messageListAdapter[args.Position]).InvokeCommand(this, x => x.ViewModel.OpenMessage);
            });

            this.WhenAnyValue(x => x.ViewModel).Subscribe(vm =>
            {
                vm.RefreshMessages.ThrownExceptions.Subscribe(ex =>
                {
                    AlertDialog.Builder dialog = new AlertDialog.Builder(Activity);
                    dialog.SetMessage("Hiba történt a frissítés során. Ellenőrizd az internetkapcsolatodat és próbáld újra.");
                    dialog.SetTitle("Hiba");
                    dialog.SetPositiveButton("Ok", (s, e) => {});
                    dialog.SetCancelable(true);
                    dialog.Create().Show();
                });
            });
            
            this.OneWayBind(ViewModel, x => x.IsRefreshing, x => x.SwipeRefresh.Refreshing);
        }

        public void OnRefresh()
        {
            ((ICommand)ViewModel.RefreshMessages).Execute(null);
        }
    }
}