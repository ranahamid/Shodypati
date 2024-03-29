(function() {
    var c = window.AmCharts;
    c.AmGanttChart = c.Class({
        inherits: c.AmSerialChart,
        construct: function(a) {
            this.type = "gantt";
            c.AmGanttChart.base.construct.call(this, a);
            this.cname = "AmGanttChart";
            this.period = "ss"
        },
        initChart: function() {
            this.processGanttData();
            this.dataChanged = !0;
            c.AmGanttChart.base.initChart.call(this)
        },
        parseData: function() {
            c.AmSerialChart.base.parseData.call(this);
            this.parseSerialData(this.ganttDataProvider)
        },
        processGanttData: function() {
            var a;
            this.graphs = [];
            var x = this.dataProvider;
            this.ganttDataProvider =
                [];
            var y = this.categoryField,
                B = this.startField,
                C = this.endField,
                D = this.durationField,
                E = this.startDateField,
                F = this.endDateField,
                u = this.colorField,
                f = this.period,
                p = c.getDate(this.startDate, this.dataDateFormat, "fff");
            this.categoryAxis.gridPosition = "start";
            a = this.valueAxis;
            this.valueAxes = [a];
            var z;
            "date" == a.type && (z = !0);
            a.minimumDate && (a.minimumDate = c.getDate(a.minimumDate, t, f));
            a.maximumDate && (a.maximumDate = c.getDate(a.maximumDate, t, f));
            isNaN(a.minimum) ||
            (a.minimumDate = c.changeDate(new Date(p),
                f,
                a.minimum,
                !0,
                !0));
            isNaN(a.maximum) || (a.maximumDate = c.changeDate(new Date(p), f, a.maximum, !0, !0));
            var t = this.dataDateFormat;
            for (a = 0; a < x.length; a++) {
                var e = x[a], l = {};
                l[y] = e[y];
                var v = e[this.segmentsField], m;
                this.ganttDataProvider.push(l);
                e = e[u];
                this.colors[a] || (this.colors[a] = c.randomColor());
                if (v)
                    for (var g = 0; g < v.length; g++) {
                        var d = v[g], b = d[B], h = d[C], n = d[D];
                        isNaN(b) && (b = m);
                        isNaN(n) || (h = b + n);
                        var n = "start_" + a + "_" + g, w = "end_" + a + "_" + g;
                        l[n] = b;
                        l[w] = h;
                        var q =
                                "lineColor color alpha fillColors description bullet customBullet bulletSize bulletConfig url labelColor dashLength pattern gap className"
                                    .split(" "),
                            k,
                            r;
                        for (r in q)
                            k = q[r] + "Field", (m = this.graph[k]) &&
                                void 0 !== d[m] &&
                                (l[m + "_" + a + "_" + g] = d[m]);
                        m = h;
                        if (z) {
                            k = c.getDate(d[E], t, f);
                            var A = c.getDate(d[F], t, f);
                            p &&
                            (isNaN(b) || (k = c.changeDate(new Date(p), f, b, !0, !0)), isNaN(h) ||
                                (A = c.changeDate(new Date(p), f, h, !0, !0)));
                            l[n] = k.getTime();
                            l[w] = A.getTime()
                        }
                        h = {};
                        c.copyProperties(d, h);
                        b = {};
                        c.copyProperties(this.graph, b, !0);
                        for (r in q) k = q[r] + "Field", this.graph[k] && (b[k] = q[r] + "_" + a + "_" + g);
                        b.customData = h;
                        b.segmentData = d;
                        b.labelFunction = this.graph.labelFunction;
                        b.balloonFunction =
                            this.graph.balloonFunction;
                        b.customBullet = this.graph.customBullet;
                        b.type = "column";
                        b.openField = n;
                        b.valueField = w;
                        b.clustered = !1;
                        d[u] && (e = d[u]);
                        b.columnWidth = d[this.columnWidthField];
                        void 0 === e && (e = this.colors[a]);
                        (d = this.brightnessStep) && (e = c.adjustLuminosity(e, g * d / 100));
                        void 0 === this.graph.lineColor && (b.lineColor = e);
                        void 0 === this.graph.fillColors && (b.fillColors = e);
                        this.graphs.push(b)
                    }
            }
        }
    })
})();