using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

using SkiaSharp.Views.Forms;
using SkiaSharp;
using System.Linq;
using System.Collections.Generic;

using MyStocks.Models;
using MyStocks.ViewModels;

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
        }
        
        private float getMax()
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

        private float getMin()
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
            base.OnAppearing();
            await history.GetHistory();
            Graph.InvalidateSurface();
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            if (history.Loading)
                return;

            var companies = history.CompaniesHistory.ToList();

            if (companies.Count == 0 || companies == null)
                return;

            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            // Clears canvas
            canvas.Clear();

            // Colors   
            SKPaint grayColor = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.Gray.ToSKColor(),
                StrokeWidth = 1
            };

            SKPaint lightGrayColor = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.LightGray.ToSKColor(),
                StrokeWidth = 1
            };

            SKPaint yellowColor = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Color.FromRgba(240, 236, 213, 180).ToSKColor(),
            };

            SKPaint blueColor = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Color.FromRgba(219, 237, 242, 180).ToSKColor()
            };

            SKPaint yellowTrace = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.FromRgba(230, 201, 75, 255).ToSKColor(),
                StrokeWidth = 4
            };

            SKPaint blueTrace = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.FromRgba(45, 195, 214, 255).ToSKColor(),
                StrokeWidth = 4
            };

            SKPaint white = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.FromRgba(255, 255, 255, 0).ToSKColor(),
                StrokeWidth = 1
            };

            SKPaint textColor = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Color.Black.ToSKColor(),
                TextSize = 30,
                TextAlign = SKTextAlign.Center
            };

            SKPaint yAxis = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Color.Gray.ToSKColor(),
                TextSize = 10
            };

            List<SKPaint> fillColors = new List<SKPaint>();
            fillColors.Add(yellowColor);
            fillColors.Add(blueColor);

            List<SKPaint> traceColors = new List<SKPaint>();
            traceColors.Add(yellowTrace);
            traceColors.Add(blueTrace);

            // Get Metrics
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;

            // Width (in pixels)
            var width = mainDisplayInfo.Width;

            // Height (in pixels)
            var height = mainDisplayInfo.Height;

            var width_ratio = width/720;
            var height_ratio = height/1280;
            
            var minValue = getMin() - 1;
            var maxValue = getMax() + 1;
            var valueDifference = maxValue - minValue;
            var horScale = 20f * (float)width_ratio;
            var vertScale = 200f * (float)height_ratio;
            var realVertScale = vertScale + 10 * (float)height_ratio;

            for (int j = 0; j < 5; j++)
            {
                canvas.DrawText((minValue + (j * valueDifference) / 4).ToString(), 21f * horScale, (vertScale * ((4 - j) / (float)4)) + 14, yAxis);
                canvas.DrawLine(0, (vertScale * ((4 - j) / (float)4)) + 10, 21f * horScale, (vertScale * ((4 - j) / (float)4)) + 10, lightGrayColor);
            }

            for (int k = 0; k < companies.Count; k++)
            {
                var company = companies[k];

                var invisiblePath = new SKPath();
                using (var path = new SKPath())
                {
                    var localMinValue = companies[k].results.OrderBy(x => x.close).First().close - 1;
                    var localMaxValue = companies[k].results.OrderByDescending(x => x.close).First().close + 1;
                    var localValueDifference = localMaxValue - localMinValue;

                    int x0 = 0;
                    int y0 = (int)(realVertScale);

                    invisiblePath.MoveTo(0, y0);

                    // Draw Horizontal Axis
                    canvas.DrawLine(x0, y0, 21f * horScale, y0, grayColor);

                    // Draw Vertical Line
                    canvas.DrawLine(21f * horScale, realVertScale, 21f * horScale, 10, grayColor);
                    
                    int i;

                    for (i = 0; i < company.results.Count; i++)
                    {
                        if (i == 0)
                        {
                            path.MoveTo(i * (21f * horScale) / (company.results.Count - 1), ((company.results[i].close - localMinValue) * vertScale / localValueDifference) + (realVertScale - vertScale));
                        }
                        else
                        {
                            path.LineTo(i * (21f * horScale) / (company.results.Count - 1), ((company.results[i].close - localMinValue) * vertScale / localValueDifference) + (realVertScale - vertScale));
                        }
                        invisiblePath.LineTo(i * (21f * horScale) / (company.results.Count - 1), ((company.results[i].close - localMinValue) * vertScale / localValueDifference) + (realVertScale - vertScale));
                    }

                    grayColor.Color = Color.Black.ToSKColor();
                    grayColor.StrokeWidth = 2;
                    canvas.DrawPath(path, traceColors[k]);
                    invisiblePath.LineTo(i * (21f * horScale) / (company.results.Count), vertScale + (realVertScale - vertScale));
                    canvas.DrawPath(invisiblePath, white);
                    canvas.DrawPath(invisiblePath, fillColors[k]);
                }
            }
        }
    }
}