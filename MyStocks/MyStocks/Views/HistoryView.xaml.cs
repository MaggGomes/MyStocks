using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

using System.Threading.Tasks;
using SkiaSharp.Views.Forms;
using SkiaSharp;
using System.Linq;
using System.Collections.Generic;

using MyStocks.Models;
using MyStocks.ViewModels;

using System.Diagnostics;

namespace MyStocks.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryView : ContentPage
    {
        HistoryViewModel history;
        List<Company> CompaniesSelected;
        String date;

        public HistoryView(List<Company> companies, string date)
        {
            InitializeComponent();
            this.CompaniesSelected = companies;
            this.date = date;
            BindingContext = this.history = new HistoryViewModel(companies, date);
            history.CompaniesHistory.CollectionChanged += StockDetails_CollectionChanged;
        }

        private void StockDetails_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Debug.WriteLine("colleantes");
            if (!history.Loading)
            {
                Debug.WriteLine("coldepois");
                Graph.InvalidateSurface();
            }
                
        }
        
        private float getMaxValue()
        {
            float max = 0;
            
            for(int i=0;i< history.CompaniesHistory.Count; i++)
            {
                for(int j=0; j< history.CompaniesHistory[i].results.Count; j++)
                {
                    if (history.CompaniesHistory[i].results[j].close > max)
                        max = history.CompaniesHistory[i].results[j].close;
                }
            }
            return max;
        }

        private float getMinValue()
        {
            float min = 9999999999;
            for (int i = 0; i < history.CompaniesHistory.Count; i++)
            {
                for (int j = 0; j < history.CompaniesHistory[i].results.Count; j++)
                {
                    if (history.CompaniesHistory[i].results[j].close < min)
                        min = history.CompaniesHistory[i].results[j].close;
                }
            }
            return min;
        }

        protected async override void OnAppearing()
        {
            Debug.WriteLine("aaaaaaa");
            base.OnAppearing();
            await history.GetHistory();
            Debug.WriteLine("bbbb");

            history.Loading = false;
            Graph.InvalidateSurface();
            //await Task.Factory.StartNew(async () => { await history.GetHistory(); });
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            Debug.WriteLine("cccc");

            if (history.Loading)
                return;


            Debug.WriteLine("chegou");

            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            var device_width = mainDisplayInfo.Width;
            var device_height = mainDisplayInfo.Height;

            var width_ratio = device_width/720;
            var height_ratio = device_height/1280;

      
            canvas.Clear();
            var stockDetails = history.CompaniesHistory.ToList();

            if (stockDetails != null && stockDetails.Count > 0)
            {
                SKPaint paint = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    Color = Color.Gray.ToSKColor(),
                    StrokeWidth = 1
                };

                SKPaint cyanTracePaint = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    Color = Color.FromRgba(0, 255, 255, 255).ToSKColor(),
                    StrokeWidth = 1
                };

                SKPaint yellowTracePaint = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    Color = Color.FromRgba(255, 255, 0, 255).ToSKColor(),
                    StrokeWidth = 1
                };

                SKPaint invisiblePaint = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    Color = Color.FromRgba(255,255,255,0).ToSKColor(),
                    StrokeWidth = 1
                };

                SKPaint textPaint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Color = Color.Black.ToSKColor(),
                    TextSize = 30,
                    TextAlign = SKTextAlign.Center
                };

                SKPaint redColor = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Color = Color.FromRgba(223, 112, 112, 200).ToSKColor(),
                    TextSize = 10,
                    //TextAlign = SKTextAlign.Center
                };

                SKPaint blueColor = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Color = Color.FromRgba(86, 153, 235, 200).ToSKColor(),
                    TextSize = 10,
                    //TextAlign = SKTextAlign.Center
                };

                SKPaint scalePaint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Color = Color.Gray.ToSKColor(),
                    TextSize = 20,
                    //TextAlign = SKTextAlign.Center
                };
                
                List<SKPaint> fillColors = new List<SKPaint>();
                fillColors.Add(redColor);
                fillColors.Add(blueColor);

                List<SKPaint> traceColors = new List<SKPaint>();
                traceColors.Add(cyanTracePaint);
                traceColors.Add(yellowTracePaint);

                var minValue = getMinValue() - 1;
                var maxValue = getMaxValue() + 1;
                var valueDifference = maxValue - minValue;
                var horScale = 30f * (float)width_ratio;
                var vertScale = 200f * (float)height_ratio;
                var realVertScale = vertScale + 10 * (float)height_ratio;

                for (int j = 0; j < 5; j++)
                {
                    canvas.DrawText((minValue + (j * valueDifference) / 4).ToString(), 21f * horScale, (vertScale * ((4 - j) / (float)4)) + 14, scalePaint);
                }

                for (int k = 0;k< stockDetails.Count; k++)
                {

                    var stockDetail = stockDetails[k];

                    var invisiblePath = new SKPath();
                    using (var path = new SKPath())
                    {
                        var localMinValue = stockDetails[k].results.OrderBy(x => x.close).First().close - 1;
                        var localMaxValue = stockDetails[k].results.OrderByDescending(x => x.close).First().close + 1;
                        var localValueDifference = localMaxValue - localMinValue;
                    
                        int x0 = 0;
                        int y0 = (int)(realVertScale);

                        invisiblePath.MoveTo(0, y0);

                        // Draw Horizontal Axis
                        canvas.DrawLine(x0, y0, 21f * horScale, y0, paint);
                        Debug.WriteLine("tamanho " + stockDetail.results.Count);

                        // Draw Vertical Line
                        canvas.DrawLine(21f * horScale, realVertScale, 21f * horScale, 10, paint);


                        int i;

                        for (i = 0; i < stockDetail.results.Count; i++)
                        {
                            Debug.WriteLine("tou a desenhar " + stockDetail.results[i].close);
                            if (i == 0)
                            {
                                path.MoveTo(i * (21f * horScale) / (stockDetail.results.Count - 1), ((stockDetail.results[i].close - localMinValue) * vertScale / localValueDifference) + (realVertScale - vertScale));
                            }
                            else
                            {
                                path.LineTo(i * (21f * horScale) / (stockDetail.results.Count - 1), ((stockDetail.results[i].close - localMinValue) * vertScale / localValueDifference) + (realVertScale - vertScale));
                            }
                            invisiblePath.LineTo(i * (21f * horScale) / (stockDetail.results.Count - 1), ((stockDetail.results[i].close - localMinValue) * vertScale / localValueDifference) + (realVertScale - vertScale));

                        }

                        paint.Color = Color.Blue.ToSKColor();
                        paint.StrokeWidth = 2;
                        canvas.DrawPath(path, traceColors[k]);
                        invisiblePath.LineTo(i * (21f * horScale) / (stockDetail.results.Count), vertScale + (realVertScale - vertScale));
                        canvas.DrawPath(invisiblePath, invisiblePaint);
                        canvas.DrawPath(invisiblePath, fillColors[k]);
                    }
                } 
            }
        }
    }
}