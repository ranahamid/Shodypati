(function() {
    var c = window.AmCharts;
    c.GaugeAxis = c.Class({
        construct: function(a) {
            this.cname = "GaugeAxis";
            this.radius = "95%";
            this.createEvents("rollOverBand", "rollOutBand", "clickBand");
            this.labelsEnabled = !0;
            this.startAngle = -120;
            this.endAngle = 120;
            this.startValue = 0;
            this.endValue = 200;
            this.gridCount = 5;
            this.tickLength = 10;
            this.minorTickLength = 5;
            this.tickColor = "#555555";
            this.labelFrequency = this.tickThickness = this.tickAlpha = 1;
            this.inside = !0;
            this.labelOffset = 10;
            this.showLastLabel = this.showFirstLabel = !0;
            this.axisThickness =
                1;
            this.axisColor = "#000000";
            this.axisAlpha = 1;
            this.gridInside = !0;
            this.topTextYOffset = 0;
            this.topTextBold = !0;
            this.bottomTextYOffset = 0;
            this.bottomTextBold = !0;
            this.centerY = this.centerX = "0%";
            this.bandOutlineAlpha = this.bandOutlineThickness = 0;
            this.bandOutlineColor = "#000000";
            this.bandAlpha = 1;
            this.bcn = "gauge-axis";
            c.applyTheme(this, a, "GaugeAxis")
        },
        value2angle: function(a) {
            return(a - this.startValue) / (this.endValue - this.startValue) * (this.endAngle - this.startAngle) +
                this.startAngle
        },
        setTopText: function(a) {
            if (void 0 !==
                a) {
                this.topText = a;
                var b = this.chart;
                if (this.axisCreated) {
                    this.topTF && this.topTF.remove();
                    var e = this.topTextFontSize;
                    e || (e = b.fontSize);
                    var d = this.topTextColor;
                    d || (d = b.color);
                    a = c.text(b.container, a, d, b.fontFamily, e, void 0, this.topTextBold);
                    c.setCN(b, a, "axis-top-label");
                    a.translate(this.centerXReal, this.centerYReal - this.radiusReal / 2 + this.topTextYOffset);
                    this.set.push(a);
                    this.topTF = a
                }
            }
        },
        setBottomText: function(a) {
            if (void 0 !== a) {
                this.bottomText = a;
                var b = this.chart;
                if (this.axisCreated) {
                    this.bottomTF && this.bottomTF.remove();
                    var e = this.bottomTextFontSize;
                    e || (e = b.fontSize);
                    var d = this.bottomTextColor;
                    d || (d = b.color);
                    a = c.text(b.container, a, d, b.fontFamily, e, void 0, this.bottomTextBold);
                    c.setCN(b, a, "axis-bottom-label");
                    a.translate(this.centerXReal, this.centerYReal + this.radiusReal / 2 + this.bottomTextYOffset);
                    this.bottomTF = a;
                    this.set.push(a)
                }
            }
        },
        draw: function() {
            var a = this.chart, b = a.container.set();
            this.set = b;
            c.setCN(a, b, this.bcn);
            c.setCN(a, b, this.bcn + "-" + this.id);
            a.graphsSet.push(b);
            var e = this.startValue, d = this.endValue, g = this.valueInterval;
            isNaN(g) && (g = (d - e) / this.gridCount);
            var n = this.minorTickInterval;
            isNaN(n) && (n = g / 5);
            var v = this.startAngle,
                k = this.endAngle,
                h = this.tickLength,
                d = (d - e) / g + 1,
                w = (k - v) / (d - 1),
                f = w / g;
            this.singleValueAngle = f;
            var p = a.container,
                G = this.tickColor,
                E = this.tickAlpha,
                M = this.tickThickness,
                n = g / n,
                N = w / n,
                H = this.minorTickLength,
                I = this.labelFrequency,
                y = this.radiusReal;
            this.inside || (y -= 15);
            var C = a.centerX + c.toCoordinate(this.centerX, a.realWidth),
                D = a.centerY + c.toCoordinate(this.centerY, a.realHeight);
            this.centerXReal = C;
            this.centerYReal =
                D;
            var z = { fill: this.axisColor, "fill-opacity": this.axisAlpha, "stroke-width": 0, "stroke-opacity": 0 },
                r,
                F;
            this.gridInside ? F = r = y : (r = y - h, F = r + H);
            var t = this.bands;
            if (t)
                for (var q = 0; q < t.length; q++) {
                    var l = t[q];
                    if (l) {
                        var x = l.startValue, B = l.endValue, m = c.toCoordinate(l.radius, y);
                        isNaN(m) && (m = F);
                        var u = c.toCoordinate(l.innerRadius, y);
                        isNaN(u) && (u = m - H);
                        var J = v + f * (x - this.startValue), B = f * (B - x), A = l.outlineColor;
                        void 0 == A && (A = this.bandOutlineColor);
                        var K = l.outlineThickness;
                        isNaN(K) && (K = this.bandOutlineThickness);
                        var L =
                            l.outlineAlpha;
                        isNaN(L) && (L = this.bandOutlineAlpha);
                        x = l.alpha;
                        isNaN(x) && (x = this.bandAlpha);
                        A = { fill: l.color, stroke: A, "stroke-width": K, "stroke-opacity": L };
                        l.url && (A.cursor = "pointer");
                        m = c.wedge(p, C, D, J, B, m, m, u, 0, A);
                        c.setCN(a, m.wedge, "axis-band");
                        void 0 !== l.id && c.setCN(a, m.wedge, "axis-band-" + l.id);
                        m.setAttr("opacity", x);
                        this.set.push(m);
                        this.addEventListeners(m, l)
                    }
                }
            f = this.axisThickness / 2;
            k = c.wedge(p, C, D, v, k - v, r + f, r + f, r - f, 0, z);
            c.setCN(a, k.wedge, "axis-line");
            b.push(k);
            k = c.doNothing;
            c.isModern || (k = Math.round);
            g = c.roundTo(g, 14);
            f = c.getDecimals(g);
            for (z = 0; z < d; z++)
                if (t = e + z * g, r = v + z * w, q = k(C + y * Math.sin(r / 180 * Math.PI)), l =
                        k(D - y * Math.cos(r / 180 * Math.PI)), m = k(C + (y - h) * Math.sin(r / 180 * Math.PI)), u =
                        k(D - (y - h) * Math.cos(r / 180 * Math.PI)), q =
                        c.line(p, [q, m], [l, u], G, E, M, 0, !1, !1, !0), c.setCN(a, q, "axis-tick"), b.push(q), q =
                        -1, m = this.labelOffset, this.inside || (m = -m - h, q = 1), l =
                        Math.sin(r / 180 * Math.PI), u =
                        Math.cos(r / 180 * Math.PI), l = C + (y - h - m) * l, m = D - (y - h - m) * u, x =
                        this.fontSize, isNaN(x) && (x = a.fontSize), u = Math.sin((r - 90) / 180 * Math.PI), J =
                        Math.cos(
                            (r -
                                90) /
                            180 *
                            Math.PI), 0 < I &&
                        this.labelsEnabled &&
                        z / I == Math.round(z / I) &&
                        (this.showLastLabel || z != d - 1) &&
                        (this.showFirstLabel || 0 !== z) &&
                        (B = this.usePrefixes
                                ? c.addPrefix(t, a.prefixesOfBigNumbers, a.prefixesOfSmallNumbers, a.nf, !0)
                                : c.formatNumber(t, a.nf, f),
                            (A = this.unit) && (B = "left" == this.unitPosition ? A + B : B + A),
                            (A = this.labelFunction) && (B = A(t)), t =
                                c.text(p, B, a.color, a.fontFamily, x), c.setCN(a, t, "axis-label"), x =
                                t.getBBox(), t.translate(l + q * x.width / 2 * J, m + q * x.height / 2 * u), b.push(t)),
                    z < d - 1)
                    for (t = 1; t < n; t++)
                        u = r + N * t, q = k(C +
                            F *
                            Math.sin(u /
                                180 *
                                Math.PI)), l = k(D - F * Math.cos(u / 180 * Math.PI)), m =
                            k(C + (F - H) * Math.sin(u / 180 * Math.PI)), u =
                            k(D - (F - H) * Math.cos(u / 180 * Math.PI)), q =
                            c.line(p, [q, m], [l, u], G, E, M, 0, !1, !1, !0), c.setCN(a, q, "axis-tick-minor"), b.push(
                            q);
            this.axisCreated = !0;
            this.setTopText(this.topText);
            this.setBottomText(this.bottomText);
            a = a.graphsSet.getBBox();
            this.width = a.width;
            this.height = a.height
        },
        addListeners: function(a, b) {
            var c = this;
            b.mouseover(function(a) {}).mouseout(function(b) { c.fireEvent("rollOutBand", a, b) }).touchend(
                    function(b) {
                        c.fireEvent("clickBand",
                            a,
                            b)
                    }).touchstart(function(b) { c.fireEvent("rollOverBand", a, b) })
                .click(function(b) { c.fireEvent("clickBand", a, b) })
        },
        fireEvent: function(a, b, c) {
            a = { type: a, dataItem: b, chart: this, event: c };
            this.fire(a.type, a)
        },
        addEventListeners: function(a, b) {
            var e = this, d = e.chart;
            a.mouseover(function(a) {
                d.showBalloon(b.balloonText, b.color, !0);
                e.fireEvent("rollOverBand", b, a)
            }).mouseout(function(a) {
                d.hideBalloon();
                e.fireEvent("rollOutBand", b, a)
            }).click(function(a) {
                e.fireEvent("clickBand", b, a);
                c.getURL(b.url, d.urlTarget)
            }).touchend(function(a) {
                e.fireEvent("clickBand",
                    b,
                    a);
                c.getURL(b.url, d.urlTarget)
            })
        }
    })
})();
(function() {
    var c = window.AmCharts;
    c.GaugeArrow = c.Class({
        construct: function(a) {
            this.cname = "GaugeArrow";
            this.color = "#000000";
            this.nailAlpha = this.alpha = 1;
            this.startWidth = this.nailRadius = 8;
            this.endWidth = 0;
            this.borderAlpha = 1;
            this.radius = "90%";
            this.nailBorderAlpha = this.innerRadius = 0;
            this.nailBorderThickness = 1;
            this.frame = 0;
            c.applyTheme(this, a, "GaugeArrow")
        },
        setValue: function(a) {
            var b = this.chart;
            b
                ? b.setValue
                ? b.setValue(this, a)
                : this.previousValue = this.value = a
                : this.previousValue = this.value = a
        }
    });
    c.GaugeBand =
        c.Class({ construct: function() { this.cname = "GaugeBand" } })
})();
(function() {
    var c = window.AmCharts;
    c.AmAngularGauge = c.Class({
        inherits: c.AmChart,
        construct: function(a) {
            this.cname = "AmAngularGauge";
            c.AmAngularGauge.base.construct.call(this, a);
            this.theme = a;
            this.type = "gauge";
            this.minRadius = this.marginRight = this.marginBottom = this.marginTop = this.marginLeft = 10;
            this.faceColor = "#FAFAFA";
            this.faceAlpha = 0;
            this.faceBorderWidth = 1;
            this.faceBorderColor = "#555555";
            this.faceBorderAlpha = 0;
            this.arrows = [];
            this.axes = [];
            this.startDuration = 1;
            this.startEffect = "easeOutSine";
            this.adjustSize =
                !0;
            this.extraHeight = this.extraWidth = 0;
            c.applyTheme(this, a, this.cname)
        },
        addAxis: function(a) { this.axes.push(a) },
        formatString: function(a, b) {
            return a = c.formatValue(a,
                b,
                ["value"],
                this.nf,
                "",
                this.usePrefixes,
                this.prefixesOfSmallNumbers,
                this.prefixesOfBigNumbers)
        },
        initChart: function() {
            c.AmAngularGauge.base.initChart.call(this);
            var a;
            0 === this.axes.length && (a = new c.GaugeAxis(this.theme), this.addAxis(a));
            var b;
            for (b = 0; b < this.axes.length; b++)
                a = this.axes[b], a = c.processObject(a, c.GaugeAxis, this.theme), a.id ||
                (a.id =
                    "axisAuto" + b + "_" + (new Date).getTime()), a.chart = this, this.axes[b] = a;
            var e = this.arrows;
            for (b = 0; b < e.length; b++) {
                a = e[b];
                a = c.processObject(a, c.GaugeArrow, this.theme);
                a.id || (a.id = "arrowAuto" + b + "_" + (new Date).getTime());
                a.chart = this;
                e[b] = a;
                var d = a.axis;
                c.isString(d) && (a.axis = c.getObjById(this.axes, d));
                a.axis || (a.axis = this.axes[0]);
                isNaN(a.value) && a.setValue(a.axis.startValue);
                isNaN(a.previousValue) && (a.previousValue = a.axis.startValue)
            }
            this.setLegendData(e);
            this.drawChart();
            this.totalFrames = 1E3 *
                this.startDuration /
                c.updateRate
        },
        drawChart: function() {
            c.AmAngularGauge.base.drawChart.call(this);
            var a = this.container, b = this.updateWidth();
            this.realWidth = b;
            var e = this.updateHeight();
            this.realHeight = e;
            var d = c.toCoordinate,
                g = d(this.marginLeft, b),
                n = d(this.marginRight, b),
                v = d(this.marginTop, e) + this.getTitleHeight(),
                k = d(this.marginBottom, e),
                h = d(this.radius, b, e),
                d = b - g - n,
                w = e - v - k + this.extraHeight;
            h || (h = Math.min(d, w) / 2);
            h < this.minRadius && (h = this.minRadius);
            this.radiusReal = h;
            this.centerX = (b - g - n) / 2 + g;
            this.centerY = (e - v - k) / 2 +
                v +
                this.extraHeight /
                2;
            isNaN(this.gaugeX) || (this.centerX = this.gaugeX);
            isNaN(this.gaugeY) || (this.centerY = this.gaugeY);
            var b = this.faceAlpha, e = this.faceBorderAlpha, f;
            if (0 < b || 0 < e)
                f = c.circle(a, h, this.faceColor, b, this.faceBorderWidth, this.faceBorderColor, e, !1),
                    f.translate(this.centerX, this.centerY), f.toBack(), (a = this.facePattern) &&
                        f.pattern(a, NaN, this.path);
            for (b = h = a = 0; b < this.axes.length; b++)
                e = this.axes[b], g = e.radius, e.radiusReal = c.toCoordinate(g, this.radiusReal), e.draw(), n =
                    1, -1 !== String(g).indexOf("%") &&
                (n = 1 +
                    (100 -
                        Number(g.substr(0,
                            g.length - 1))) /
                    100), e.width * n > a && (a = e.width * n), e.height * n > h && (h = e.height * n);
            (b = this.legend) && b.invalidateSize();
            if (this.adjustSize && !this.chartCreated) {
                f && (f = f.getBBox(), f.width > a && (a = f.width), f.height > h && (h = f.height));
                f = 0;
                if (w > h || d > a) f = Math.min(w - h, d - a);
                0 < f && (this.extraHeight = w - h, this.chartCreated = !0, this.validateNow())
            }
            d = this.arrows.length;
            for (b = 0; b < d; b++) w = this.arrows[b], w.drawnAngle = NaN;
            this.dispDUpd()
        },
        validateSize: function() {
            this.extraHeight = this.extraWidth = 0;
            this.chartCreated = !1;
            c.AmAngularGauge.base.validateSize.call(this)
        },
        addArrow: function(a) { this.arrows.push(a) },
        removeArrow: function(a) {
            c.removeFromArray(this.arrows, a);
            this.validateNow()
        },
        removeAxis: function(a) {
            c.removeFromArray(this.axes, a);
            this.validateNow()
        },
        drawArrow: function(a, b) {
            a.set && a.set.remove();
            var e = this.container;
            a.set = e.set();
            c.setCN(this, a.set, "gauge-arrow");
            c.setCN(this, a.set, "gauge-arrow-" + a.id);
            if (!a.hidden) {
                var d = a.axis,
                    g = d.radiusReal,
                    n = d.centerXReal,
                    v = d.centerYReal,
                    k = a.startWidth,
                    h = a.endWidth,
                    w = c.toCoordinate(a.innerRadius, d.radiusReal),
                    f = c.toCoordinate(a.radius,
                        d.radiusReal);
                d.inside || (f -= 15);
                var p = a.nailColor;
                p || (p = a.color);
                var G = a.nailColor;
                G || (G = a.color);
                0 < a.nailRadius &&
                (p = c.circle(e, a.nailRadius, p, a.nailAlpha, a.nailBorderThickness, p, a.nailBorderAlpha),
                    c.setCN(this, p, "gauge-arrow-nail"), a.set.push(p), p.translate(n, v));
                isNaN(f) && (f = g - d.tickLength);
                var d = Math.sin(b / 180 * Math.PI),
                    g = Math.cos(b / 180 * Math.PI),
                    p = Math.sin((b + 90) / 180 * Math.PI),
                    E = Math.cos((b + 90) / 180 * Math.PI),
                    e = c.polygon(e,
                        [n - k / 2 * p + w * d, n + f * d - h / 2 * p, n + f * d + h / 2 * p, n + k / 2 * p + w * d],
                        [
                            v + k / 2 * E - w * g, v - f * g + h / 2 * E,
                            v - f * g - h / 2 * E, v - k / 2 * E - w * g
                        ],
                        a.color,
                        a.alpha,
                        1,
                        G,
                        a.borderAlpha,
                        void 0,
                        !0);
                c.setCN(this, e, "gauge-arrow");
                a.set.push(e);
                this.graphsSet.push(a.set)
            }
        },
        setValue: function(a, b) {
            a.axis && a.axis.value2angle && (a.frame = 0, a.previousValue = a.value);
            a.value = b;
            var c = this.legend;
            c && c.updateValues()
        },
        handleLegendEvent: function(a) {
            var b = a.type;
            a = a.dataItem;
            if (!this.legend.data && a)
                switch (b) {
                case "hideItem":
                    this.hideArrow(a);
                    break;
                case "showItem":
                    this.showArrow(a)
                }
        },
        hideArrow: function(a) {
            a.set.hide();
            a.hidden = !0
        },
        showArrow: function(a) {
            a.set.show();
            a.hidden = !1
        },
        updateAnimations: function() {
            c.AmAngularGauge.base.updateAnimations.call(this);
            for (var a = this.arrows.length, b, e = 0; e < a; e++)
                if (b = this.arrows[e], b.axis && b.axis.value2angle) {
                    var d;
                    b.frame >= this.totalFrames
                        ? d = b.value
                        : (b.frame++, b.clockWiseOnly &&
                            b.value < b.previousValue &&
                            (d = b.axis, b.previousValue -= d.endValue - d.startValue), d =
                            c.getEffect(this.startEffect), d =
                            c[d](0, b.frame, b.previousValue, b.value - b.previousValue, this.totalFrames), isNaN(d) &&
                            (d = b.value));
                    d = b.axis.value2angle(d);
                    b.drawnAngle != d &&
                    (this.drawArrow(b,
                        d), b.drawnAngle = d)
                }
        }
    })
})();