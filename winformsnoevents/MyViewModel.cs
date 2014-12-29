﻿using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace winformsnoevents
{
    public class MyViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<string> backgroundTicker;
        public string BackgroundTicker
        {
            get
            {
                return backgroundTicker.Value;
            }
            set {/* read-only properties not implemented yet?*/}
        }

        private readonly ObservableAsPropertyHelper<int> wordCount;
        public int WordCount
        {
            get
            {
                return wordCount.Value;
            }
            set { }
        }

        public MyViewModel(SynchronizationContext context,IObservable<string> input)
        {
            Observable.Interval(TimeSpan.FromSeconds(1))
               .ObserveOn(context)
               .Select(_ => DateTime.Now.ToLongTimeString())
               .ToProperty(this, x => x.BackgroundTicker,out backgroundTicker);

            input
                .Where(x => x != null)
                .Select(s => s.Split().Where(x => x.Trim().Length > 0).Count())
                .Throttle(TimeSpan.FromSeconds(0.5))
                .ObserveOn(context)
                .ToProperty(this, x => x.WordCount, out wordCount);
        }
    }
}
