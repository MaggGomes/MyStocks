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

using System.Diagnostics;

namespace MyStocks.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryView : ContentPage
    {
        HistoryViewModel history;
        List<Company> CompaniesSelected;
        String date;
        int numberOfPoints;

        public HistoryView(List<Company> companies, string date, int numberOfPoints)
        {
            InitializeComponent();
            this.CompaniesSelected = companies;
            this.date = date;
            this.numberOfPoints = numberOfPoints;
            BindingContext = this.history = new HistoryViewModel(companies, date);
        }
        
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await history.GetQuote();
            await history.GetHistory();
            Graph.InvalidateSurface();
        }

        private float getMax()
        {
            var companies = history.CompaniesHistory.ToList();
            float maxQuote = 0;

            for (int i = 0; i < companies.Count; i++)
            {
                var quote = companies[i].results.OrderByDescending(x => x.close).First().close;

                if (quote > maxQuote)
                {
                    maxQuote = quote;
                }
            }

            return maxQuote;
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            if (history.Loading)
                return;

            var companies = history.CompaniesHistory.ToList();
            var currentQuotes = history.CompaniesQuotes.ToList();
            

            if (companies.Count == 0 || companies == null)
                return;

            Company1Text.Text = CompaniesSelected[0].Symbol;
            Company1Quote.Text = currentQuotes[0].results[0].lastPrice + " USD";
            
            if (currentQuotes[0].results[0].percentChange >= 0)
            {
                Company1Percent.Text = "+"+currentQuotes[0].results[0].percentChange + "%";
                Company1Percent.TextColor = Color.DarkGreen;
            }

            else
            {
                Company1Percent.Text = currentQuotes[0].results[0].percentChange + "%";
                Company1Percent.TextColor = Color.DarkRed;
            }

            Company1.IsVisible = true;

            if(companies.Count == 2)
            {
                Company2Text.Text = CompaniesSelected[1].Symbol;
                Company2Quote.Text = currentQuotes[0].results[1].lastPrice + " USD";

                if (currentQuotes[0].results[1].percentChange >= 0)
                {
                    Company2Percent.Text = "+" + currentQuotes[0].results[1].percentChange + "%";
                    Company2Percent.TextColor = Color.DarkGreen;
                }

                else
                {
                    Company2Percent.Text = currentQuotes[0].results[1].percentChange + "%";
                    Company2Percent.TextColor = Color.DarkRed;
                }

                Company2.IsVisible = true;
            }

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
                Color = Color.FromRgba(240, 236, 213, 120).ToSKColor(),
            };

            SKPaint blueColor = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Color.FromRgba(219, 237, 242, 120).ToSKColor()
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
                Color = Color.Gray.ToSKColor(),
                TextSize = 20
            };

            SKPaint smallTextColor = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Color.Gray.ToSKColor(),
                TextSize = 14
            };

            SKPaint axisColor = new SKPaint
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

            // Chart size
            var chartWidth = (float)width - 100f;
            var chartHeight = (float)height / 2;

            var maxQuote = getMax();

            for (int k = 0; k < companies.Count; k++)
            {
                var invisiblePath = new SKPath();
                using (var path = new SKPath())
                {
                    invisiblePath.MoveTo(0, chartHeight);

                    for (int i = 0; i < companies[k].results.Count; i++)
                    {
                        invisiblePath.LineTo(i * chartWidth / (companies[k].results.Count - 1), ((maxQuote - companies[k].results[i].close)*chartHeight)/ maxQuote);
                       
                        if (i == 0)
                        {
                            path.MoveTo(i * chartWidth / (companies[k].results.Count - 1), ((maxQuote - companies[k].results[i].close) * chartHeight) / maxQuote);
                        }

                        else
                        {
                            path.LineTo(i * chartWidth / (companies[k].results.Count - 1), ((maxQuote - companies[k].results[i].close) * chartHeight) / maxQuote);
                        }
                    }

                    invisiblePath.LineTo(chartWidth, chartHeight);
                    canvas.DrawPath(path, traceColors[k]);
                    canvas.DrawPath(invisiblePath, white);
                    canvas.DrawPath(invisiblePath, fillColors[k]);
                }
            }
            
            // X axis labels
            canvas.DrawText(companies[0].results[0].timestamp.ToString("dd/MM/yyyy"), 0, chartHeight + 30, smallTextColor);
            canvas.DrawText(companies[0].results.Last().timestamp.ToString("dd/MM/yyyy"), chartWidth, chartHeight + 30, smallTextColor);
            
            // Y axis labels
            for (int i = 0; i < 6; i++)
            {
                if(i > 0)
                {
                    canvas.DrawText(Math.Round((i * maxQuote) / 5, 2).ToString(), chartWidth + 5, (chartHeight * ((5 - i) / (float)5)) + 15, textColor);
                }
                
                canvas.DrawLine(0, (chartHeight * ((5 - i) / (float)5)), chartWidth, (chartHeight * ((5 - i) / (float)5)), lightGrayColor);
            }
        }

        async void SeeDetails1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CompanyView(history.CompaniesQuotes.ToList()[0].results[0]));

        }

        async void SeeDetails2(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CompanyView(history.CompaniesQuotes.ToList()[0].results[1]));
        }
    }
}