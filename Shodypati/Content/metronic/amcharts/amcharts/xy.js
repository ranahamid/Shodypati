(function() {
    var e = window.AmCharts;
    e.AmXYChart = e.Class({
        inherits: e.AmRectangularChart,
        construct: function(a) {
            this.type = "xy";
            e.AmXYChart.base.construct.call(this, a);
            this.cname = "AmXYChart";
            this.theme = a;
            this.createEvents("zoomed");
            this.maxZoomFactor = 20;
            e.applyTheme(this, a, this.cname)
        },
        initChart: function() {
            e.AmXYChart.base.initChart.call(this);
            this.dataChanged && (this.updateData(), this.dataChanged = !1, this.dispatchDataUpdated = !0);
            this.updateScrollbar = !0;
            this.drawChart();
            this.autoMargins &&
                !this.marginsUpdated &&
                (this.marginsUpdated = !0, this.measureMargins());
            var a = this.marginLeftReal, c = this.marginTopReal, b = this.plotAreaWidth, d = this.plotAreaHeight;
            this.graphsSet.clipRect(a, c, b, d);
            this.bulletSet.clipRect(a, c, b, d);
            this.trendLinesSet.clipRect(a, c, b, d)
        },
        prepareForExport: function() {
            var a = this.bulletSet;
            a.clipPath && this.container.remove(a.clipPath)
        },
        createValueAxes: function() {
            var a = [], c = [];
            this.xAxes = a;
            this.yAxes = c;
            var b = this.valueAxes, d, f;
            for (f = 0; f < b.length; f++) {
                d = b[f];
                var k = d.position;
                if ("top" == k || "bottom" == k)
                    d.rotate =
                        !0;
                d.setOrientation(d.rotate);
                k = d.orientation;
                "V" == k && c.push(d);
                "H" == k && a.push(d)
            }
            0 === c.length &&
                (d = new e.ValueAxis(this.theme), d.rotate = !1, d.setOrientation(!1), b.push(d), c.push(d));
            0 === a.length &&
                (d = new e.ValueAxis(this.theme), d.rotate = !0, d.setOrientation(!0), b.push(d), a.push(d));
            for (f = 0; f < b.length; f++) this.processValueAxis(b[f], f);
            a = this.graphs;
            for (f = 0; f < a.length; f++) this.processGraph(a[f], f)
        },
        drawChart: function() {
            e.AmXYChart.base.drawChart.call(this);
            e.ifArray(this.chartData)
                ? (this.chartScrollbar &&
                        this.updateScrollbars(), this.selfZoom &&
                    (this.horizontalPosition =
                        this.horizontalPosition * this.plotAreaWidth / this.prevPlotAreaWidth, this.verticalPosition =
                        this.verticalPosition * this.plotAreaHeight / this.prevPlotAreaHeight, this.selfZoom = !1),
                    this.zoomChart())
                : this.cleanChart();
            if (this.hideXScrollbar) {
                var a = this.scrollbarH;
                a && (this.removeListener(a, "zoomed", this.handleHSBZoom), a.destroy());
                this.scrollbarH = null
            }
            if (this.hideYScrollbar) {
                if (a = this.scrollbarV)
                    this.removeListener(a, "zoomed", this.handleVSBZoom),
                        a.destroy();
                this.scrollbarV = null
            }
            if (!this.autoMargins || this.marginsUpdated) this.dispDUpd(), this.chartCreated = !0, this.zoomScrollbars()
        },
        cleanChart: function() {
            e.callMethod("destroy", [this.valueAxes, this.graphs, this.scrollbarV, this.scrollbarH, this.chartCursor])
        },
        zoomChart: function() {
            this.toggleZoomOutButton();
            this.zoomObjects(this.valueAxes);
            this.zoomObjects(this.graphs);
            this.zoomTrendLines();
            this.dispatchAxisZoom();
            this.prevPlotAreaWidth = this.plotAreaWidth;
            this.prevPlotAreaHeight = this.plotAreaHeight
        },
        toggleZoomOutButton: function() {
            1 == this.heightMultiplier && 1 == this.widthMultiplier ? this.showZB(!1) : this.showZB(!0)
        },
        dispatchAxisZoom: function() {
            var a = this.valueAxes, c;
            for (c = 0; c < a.length; c++) {
                var b = a[c];
                if (!isNaN(b.min) && !isNaN(b.max)) {
                    var d, f;
                    "V" == b.orientation
                        ? (d = b.coordinateToValue(-this.verticalPosition), f =
                            b.coordinateToValue(-this.verticalPosition + this.plotAreaHeight))
                        : (d = b.coordinateToValue(-this.horizontalPosition), f =
                            b.coordinateToValue(-this.horizontalPosition + this.plotAreaWidth));
                    if (!isNaN(d) &&
                        !isNaN(f)) {
                        if (d > f) {
                            var e = f;
                            f = d;
                            d = e
                        }
                        b.dispatchZoomEvent(d, f)
                    }
                }
            }
        },
        zoomObjects: function(a) {
            var c = a.length, b, d;
            for (b = 0; b < c; b++)
                d = a[b], d.minTemp = d.min, d.maxTemp =
                    d.max, this.updateObjectSize(d), d.zoom(0, this.chartData.length - 1), d.minTemp = NaN, d.maxTemp =
                    NaN
        },
        updateData: function() {
            this.parseData();
            var a = this.chartData,
                c = a.length - 1,
                b = this.graphs,
                d = this.dataProvider,
                e = -Infinity,
                k = Infinity,
                h,
                l;
            for (h = 0; h < b.length; h++)
                if (l = b[h], l.data = a, l.zoom(0, c), l = l.valueField) {
                    var n;
                    for (n = 0; n < d.length; n++)
                        if (null !== p) {
                            var p =
                                Number(d[n][l]);
                            p > e && (e = p);
                            p < k && (k = p)
                        }
                }
            for (h = 0; h < b.length; h++) l = b[h], l.maxValue = e, l.minValue = k;
            if (a = this.chartCursor) a.updateData(), a.type = "crosshair", a.valueBalloonsEnabled = !1
        },
        zoomOut: function() {
            this.verticalPosition = this.horizontalPosition = 0;
            this.heightMultiplier = this.widthMultiplier = 1;
            this.zoomChart();
            this.zoomScrollbars()
        },
        processValueAxis: function(a) {
            a.chart = this;
            a.minMaxField = "H" == a.orientation ? "x" : "y";
            a.min = NaN;
            a.max = NaN;
            this.listenTo(a, "axisSelfZoomed", this.handleAxisSelfZoom)
        },
        processGraph: function(a) {
            e.isString(a.xAxis) &&
                (a.xAxis = this.getValueAxisById(a.xAxis));
            e.isString(a.yAxis) && (a.yAxis = this.getValueAxisById(a.yAxis));
            a.xAxis || (a.xAxis = this.xAxes[0]);
            a.yAxis || (a.yAxis = this.yAxes[0]);
            a.valueAxis = a.yAxis
        },
        parseData: function() {
            e.AmXYChart.base.parseData.call(this);
            this.chartData = [];
            var a = this.dataProvider, c = this.valueAxes, b = this.graphs, d;
            if (a)
                for (d = 0; d < a.length; d++) {
                    var f = { axes: {}, x: {}, y: {} }, k = this.dataDateFormat, h = a[d], l;
                    for (l = 0; l < c.length; l++) {
                        var n = c[l].id;
                        f.axes[n] = {};
                        f.axes[n].graphs = {};
                        var p;
                        for (p = 0; p < b.length; p++) {
                            var m =
                                    b[p],
                                t = m.id;
                            if (m.xAxis.id == n || m.yAxis.id == n) {
                                var q = {};
                                q.serialDataItem = f;
                                q.index = d;
                                var r = {}, g = h[m.valueField];
                                null !== g && (g = Number(g), isNaN(g) || (r.value = g));
                                g = h[m.xField];
                                null !== g &&
                                ("date" == m.xAxis.type && (g = e.getDate(h[m.xField], k).getTime()), g =
                                    Number(g), isNaN(g) || (r.x = g));
                                g = h[m.yField];
                                null !== g &&
                                ("date" == m.yAxis.type && (g = e.getDate(h[m.yField], k).getTime()), g =
                                    Number(g), isNaN(g) || (r.y = g));
                                g = h[m.errorField];
                                null !== g && (g = Number(g), isNaN(g) || (r.error = g));
                                q.values = r;
                                this.processFields(m, q, h);
                                q.serialDataItem =
                                    f;
                                q.graph = m;
                                f.axes[n].graphs[t] = q
                            }
                        }
                    }
                    this.chartData[d] = f
                }
        },
        formatString: function(a, c, b) {
            var d = c.graph, f = d.numberFormatter;
            f || (f = this.nf);
            var k, h;
            "date" == c.graph.xAxis.type &&
            (k = e.formatDate(new Date(c.values.x), d.dateFormat, this), h = RegExp("\\[\\[x\\]\\]", "g"), a =
                a.replace(h, k));
            "date" == c.graph.yAxis.type &&
            (k = e.formatDate(new Date(c.values.y), d.dateFormat, this), h = RegExp("\\[\\[y\\]\\]", "g"), a =
                a.replace(h, k));
            a = e.formatValue(a, c.values, ["value", "x", "y"], f);
            -1 != a.indexOf("[[") &&
            (a = e.formatDataContextValue(a,
                c.dataContext));
            return a = e.AmXYChart.base.formatString.call(this, a, c, b)
        },
        addChartScrollbar: function(a) {
            e.callMethod("destroy", [this.chartScrollbar, this.scrollbarH, this.scrollbarV]);
            if (a) {
                this.chartScrollbar = a;
                this.scrollbarHeight = a.scrollbarHeight;
                var c =
                    "backgroundColor backgroundAlpha selectedBackgroundColor selectedBackgroundAlpha scrollDuration resizeEnabled hideResizeGrips scrollbarHeight updateOnReleaseOnly"
                        .split(" ");
                if (!this.hideYScrollbar) {
                    var b = new e.SimpleChartScrollbar(this.theme);
                    b.skipEvent =
                        !0;
                    b.chart = this;
                    this.listenTo(b, "zoomed", this.handleVSBZoom);
                    e.copyProperties(a, b, c);
                    b.rotate = !0;
                    this.scrollbarV = b
                }
                this.hideXScrollbar ||
                (b = new e.SimpleChartScrollbar(this.theme), b.skipEvent = !0, b.chart =
                    this, this.listenTo(b, "zoomed", this.handleHSBZoom), e.copyProperties(a, b, c), b.rotate =
                    !1, this.scrollbarH = b)
            }
        },
        updateTrendLines: function() {
            var a = this.trendLines, c;
            for (c = 0; c < a.length; c++) {
                var b = a[c], b = e.processObject(b, e.TrendLine, this.theme);
                a[c] = b;
                b.chart = this;
                var d = b.valueAxis;
                e.isString(d) &&
                (b.valueAxis =
                    this.getValueAxisById(d));
                d = b.valueAxisX;
                e.isString(d) && (b.valueAxisX = this.getValueAxisById(d));
                b.id || (b.id = "trendLineAuto" + c + "_" + (new Date).getTime());
                b.valueAxis || (b.valueAxis = this.yAxes[0]);
                b.valueAxisX || (b.valueAxisX = this.xAxes[0])
            }
        },
        updateMargins: function() {
            e.AmXYChart.base.updateMargins.call(this);
            var a = this.scrollbarV;
            a && (this.getScrollbarPosition(a, !0, this.yAxes[0].position), this.adjustMargins(a, !0));
            if (a = this.scrollbarH)
                this.getScrollbarPosition(a, !1, this.xAxes[0].position), this.adjustMargins(a,
                    !1)
        },
        updateScrollbars: function() {
            e.AmXYChart.base.updateScrollbars.call(this);
            var a = this.scrollbarV;
            a && (this.updateChartScrollbar(a, !0), a.draw());
            if (a = this.scrollbarH) this.updateChartScrollbar(a, !1), a.draw()
        },
        zoomScrollbars: function() {
            var a = this.scrollbarH;
            a && a.relativeZoom(this.widthMultiplier, -this.horizontalPosition / this.widthMultiplier);
            (a = this.scrollbarV) &&
                a.relativeZoom(this.heightMultiplier, -this.verticalPosition / this.heightMultiplier)
        },
        fitMultiplier: function(a) {
            a > this.maxZoomFactor && (a = this.maxZoomFactor);
            return a
        },
        fitH: function(a, c) {
            var b = -(this.plotAreaWidth * c - this.plotAreaWidth);
            a < b && (a = b);
            this.horizontalPosition = a
        },
        fitV: function(a, c) {
            var b = -(this.plotAreaHeight * c - this.plotAreaHeight);
            a < b && (a = b);
            this.verticalPosition = a
        },
        handleHSBZoom: function(a) {
            var c = this.fitMultiplier(a.multiplier);
            this.fitH(-a.position * c, c);
            this.widthMultiplier = c;
            this.zoomChart()
        },
        handleVSBZoom: function(a) {
            var c = this.fitMultiplier(a.multiplier);
            this.fitV(-a.position * c, c);
            this.heightMultiplier = c;
            this.zoomChart()
        },
        handleAxisSelfZoom: function(a) {
            if ("H" ==
                a.valueAxis.orientation) {
                var c = this.fitMultiplier(a.multiplier);
                this.fitH(-a.position * c, c);
                this.widthMultiplier = c
            } else c = this.fitMultiplier(a.multiplier), this.fitV(-a.position * c, c), this.heightMultiplier = c;
            this.zoomChart();
            a = this.graphs;
            for (c = 0; c < a.length; c++) a[c].setAnimationPlayed();
            this.zoomScrollbars()
        },
        handleCursorZoom: function(a) {
            var c = this.widthMultiplier * this.plotAreaWidth / a.selectionWidth,
                b = this.heightMultiplier * this.plotAreaHeight / a.selectionHeight,
                c = this.fitMultiplier(c),
                b = this.fitMultiplier(b),
                d = (this.verticalPosition - a.selectionY) * b / this.heightMultiplier;
            this.fitH((this.horizontalPosition - a.selectionX) * c / this.widthMultiplier, c);
            this.fitV(d, b);
            this.widthMultiplier = c;
            this.heightMultiplier = b;
            this.zoomChart();
            this.zoomScrollbars()
        },
        removeChartScrollbar: function() {
            e.callMethod("destroy", [this.scrollbarH, this.scrollbarV]);
            this.scrollbarV = this.scrollbarH = null
        },
        handleReleaseOutside: function(a) {
            e.AmXYChart.base.handleReleaseOutside.call(this, a);
            e.callMethod("handleReleaseOutside",
                [
                    this.scrollbarH,
                    this.scrollbarV
                ])
        },
        update: function() {
            e.AmXYChart.base.update.call(this);
            this.scrollbarH && this.scrollbarH.update && this.scrollbarH.update();
            this.scrollbarV && this.scrollbarV.update && this.scrollbarV.update()
        }
    })
})();