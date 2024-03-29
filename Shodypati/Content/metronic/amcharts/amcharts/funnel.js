(function() {
    var a = window.AmCharts;
    a.AmFunnelChart = a.Class({
        inherits: a.AmSlicedChart,
        construct: function(p) {
            this.type = "funnel";
            a.AmFunnelChart.base.construct.call(this, p);
            this.cname = "AmFunnelChart";
            this.startX = this.startY = 0;
            this.baseWidth = "100%";
            this.neckHeight = this.neckWidth = 0;
            this.rotate = !1;
            this.valueRepresents = "height";
            this.pullDistance = 30;
            this.labelPosition = "center";
            this.labelText = "[[title]]: [[value]]";
            this.balloonText = "[[title]]: [[value]]\n[[description]]";
            a.applyTheme(this, p, this.cname)
        },
        drawChart: function() {
            a.AmFunnelChart.base.drawChart.call(this);
            var p = this.chartData;
            if (a.ifArray(p))
                if (0 < this.realWidth && 0 < this.realHeight) {
                    var e = Math.round(this.depth3D * Math.cos(this.angle * Math.PI / 180)),
                        l = Math.round(-this.depth3D * Math.sin(this.angle * Math.PI / 180)),
                        c = this.container,
                        d = this.startDuration,
                        f = this.rotate,
                        n = this.updateWidth();
                    this.realWidth = n;
                    var w = this.updateHeight();
                    this.realHeight = w;
                    var g = a.toCoordinate,
                        q = g(this.marginLeft, n),
                        r = g(this.marginRight, n),
                        b = g(this.marginTop, w) + this.getTitleHeight(),
                        g = g(this.marginBottom, w);
                    0 < e &&
                        0 > l &&
                        (this.neckHeight = this.neckWidth =
                            0, f ? g -= l / 2 : b -= l / 2);
                    var r = n - q - r,
                        E = a.toCoordinate(this.baseWidth, r),
                        I = a.toCoordinate(this.neckWidth, r),
                        D = w - g - b,
                        F = a.toCoordinate(this.neckHeight, D),
                        y = b + D - F;
                    f && (b = w - g, y = b - D + F);
                    this.firstSliceY = b;
                    a.VML && (this.startAlpha = 1);
                    for (var z = r / 2 + q,
                        G = (D - F) / ((E - I) / 2),
                        B = 1,
                        t = E / 2,
                        E = (D - F) * (E + I) / 2 + I * F,
                        H = b,
                        M = 0,
                        F = 0;
                        F < p.length;
                        F++) {
                        var h = p[F], u;
                        if (!0 !== h.hidden && (this.showZeroSlices || 0 !== h.percents)) {
                            var A = [], m = [], k;
                            if ("height" == this.valueRepresents) k = D * h.percents / 100;
                            else {
                                var C = -E * h.percents / 100 / 2, K = t;
                                u = -1 / (2 * G);
                                k = Math.pow(K,
                                        2) -
                                    4 * u * C;
                                0 > k && (k = 0);
                                k = (Math.sqrt(k) - K) / (2 * u);
                                if (!f && b >= y || f && b <= y) k = 2 * -C / I;
                                else if (!f && b + k > y || f && b - k < y)
                                    u = f ? Math.round(k + (b - k - y)) : Math.round(k - (b + k - y)), k = u / G, k =
                                        u + 2 * (-C - (K - k / 2) * u) / I
                            }
                            C = t - k / G;
                            K = !1;
                            !f && b + k > y || f && b - k < y
                                ? (C = I / 2, A.push(z - t, z + t, z + C, z + C, z - C, z - C), f
                                    ? (u = k + (b - k - y), b < y && (u = 0), m.push(b,
                                        b,
                                        b - u,
                                        b - k,
                                        b - k,
                                        b - u,
                                        b))
                                    : (u = k - (b + k - y), b > y && (u = 0), m.push(b,
                                        b,
                                        b + u,
                                        b + k,
                                        b + k,
                                        b + u,
                                        b)), K = !0)
                                : (A.push(z - t, z + t, z + C, z - C), f
                                    ? m.push(b, b, b - k, b - k)
                                    : m.push(b, b, b + k, b + k));
                            u = c.set();
                            0 < e && 0 > l
                                ? (m = C / t, A = -1, f || (A = 1), isNaN(B) && (B = 0), A = (new a.Cuboid(c,
                                    2 * t,
                                    A * k,
                                    e,
                                    l * B,
                                    h.color,
                                    h.alpha,
                                    this.outlineThickness,
                                    this.outlineColor,
                                    this.outlineAlpha,
                                    90,
                                    0,
                                    !1,
                                    0,
                                    h.pattern,
                                    m)).set, A.translate(z - t, b - l / 2 * B), B *= m)
                                : A = a.polygon(c,
                                    A,
                                    m,
                                    h.color,
                                    h.alpha,
                                    this.outlineThickness,
                                    this.outlineColor,
                                    this.outlineAlpha);
                            a.setCN(this, u, "funnel-item");
                            a.setCN(this, A, "funnel-slice");
                            a.setCN(this, u, h.className, !0);
                            u.push(A);
                            this.graphsSet.push(u);
                            f || u.toBack();
                            h.wedge = u;
                            h.index = F;
                            if (m = this.gradientRatio) {
                                var x = [], v;
                                for (v = 0; v < m.length; v++) x.push(a.adjustLuminosity(h.color, m[v]));
                                0 <
                                    x.length &&
                                    A.gradient("linearGradient", x);
                                h.pattern && A.pattern(h.pattern, NaN, this.path)
                            }
                            0 < d && (this.chartCreated || u.setAttr("opacity", this.startAlpha));
                            this.addEventListeners(u, h);
                            h.ty0 = b - k / 2;
                            this.labelsEnabled &&
                                this.labelText &&
                                h.percents >= this.hideLabelsPercent &&
                                (m = this.formatString(this.labelText, h), (A = this.labelFunction) && (m = A(h, m)),
                                    x = h.labelColor, x || (x = this.color), A = this.labelPosition, v =
                                        "left", "center" == A && (v = "middle"), "left" == A && (v = "right"),
                                    "" !== m &&
                                    (m = a.wrappedText(c,
                                            m,
                                            x,
                                            this.fontFamily,
                                            this.fontSize,
                                            v,
                                            !1,
                                            this.maxLabelWidth), a.setCN(this, m, "funnel-label"),
                                        a.setCN(this, m, h.className, !0), m.node.style.pointerEvents =
                                            "none", u.push(m), x =
                                            z, f
                                            ? (v = b - k / 2, h.ty0 = v)
                                            : (v = b + k / 2, h.ty0 =
                                                v, v < H + M + 5 && (v = H + M + 5), v > w - g && (v = w - g)),
                                        "right" == A &&
                                            (x = r + 10 + q, h.tx0 = z + (t - k / 2 / G), K && (h.tx0 = z + C)),
                                        "left" == A && (h.tx0 = z - (t - k / 2 / G), K && (h.tx0 = z - C), x = q), h
                                            .label =
                                            m, h.labelX = x, h.labelY = v, h.labelHeight =
                                            m.getBBox().height, m.translate(x, v), t = m.getBBox(), H =
                                            a.rect(c, t.width + 5, t.height + 5, "#ffffff", .005), H.translate(x + t.x,
                                            v + t.y), u.push(H), h.hitRect =
                                            H, M = m.getBBox().height, H = v));
                            (0 === h.alpha || 0 < d && !this.chartCreated) && u.hide();
                            b = f ? b - k : b + k;
                            t = C;
                            h.startX = a.toCoordinate(this.startX, n);
                            h.startY = a.toCoordinate(this.startY, w);
                            h.pullX = a.toCoordinate(this.pullDistance, n);
                            h.pullY = 0;
                            h.balloonX = z;
                            h.balloonY = h.ty0
                        }
                    }
                    this.arrangeLabels();
                    this.initialStart();
                    (p = this.legend) && p.invalidateSize()
                } else this.cleanChart();
            this.dispDUpd()
        },
        arrangeLabels: function() {
            var a = this.rotate, e;
            e = a ? 0 : this.realHeight;
            for (var l = 0, c = this.chartData, d = c.length, f, n = 0; n < d; n++) {
                f = c[d -
                    n -
                    1];
                var w = f.label, g = f.labelY, q = f.labelX, r = f.labelHeight, b = g;
                a ? e + l + 5 > g && (b = e + l + 5) : g + r + 5 > e && (b = e - 5 - r);
                e = b;
                l = r;
                w && (w.translate(q, b), w = w.getBBox(), f.hitRect && f.hitRect.translate(q + w.x, b + w.y));
                f.labelY = b;
                f.tx = q;
                f.ty = b;
                f.tx2 = q
            }
            "center" != this.labelPosition && this.drawTicks()
        }
    })
})();
(function() {
    var a = window.AmCharts;
    a.Cuboid = a.Class({
        construct: function(a, e, l, c, d, f, n, w, g, q, r, b, E, I, D, F, y) {
            this.set = a.set();
            this.container = a;
            this.h = Math.round(l);
            this.w = Math.round(e);
            this.dx = c;
            this.dy = d;
            this.colors = f;
            this.alpha = n;
            this.bwidth = w;
            this.bcolor = g;
            this.balpha = q;
            this.dashLength = I;
            this.topRadius = F;
            this.pattern = D;
            this.rotate = E;
            this.bcn = y;
            E ? 0 > e && 0 === r && (r = 180) : 0 > l && 270 == r && (r = 90);
            this.gradientRotation = r;
            0 === c && 0 === d && (this.cornerRadius = b);
            this.draw()
        },
        draw: function() {
            var p = this.set;
            p.clear();
            var e = this.container,
                l = e.chart,
                c = this.w,
                d = this.h,
                f = this.dx,
                n = this.dy,
                w = this.colors,
                g = this.alpha,
                q = this.bwidth,
                r = this.bcolor,
                b = this.balpha,
                E = this.gradientRotation,
                I = this.cornerRadius,
                D = this.dashLength,
                F = this.pattern,
                y = this.topRadius,
                z = this.bcn,
                G = w,
                B = w;
            "object" == typeof w && (G = w[0], B = w[w.length - 1]);
            var t, H, M, h, u, A, m, k, C, K = g;
            F && (g = 0);
            var x, v, J, L, N = this.rotate;
            if (0 < Math.abs(f) || 0 < Math.abs(n))
                if (isNaN(y))
                    m = B, B = a.adjustLuminosity(G, -.2), B = a.adjustLuminosity(G, -.2), t = a.polygon(e,
                            [0, f, c + f, c, 0],
                            [0, n, n, 0, 0],
                            B,
                            g,
                            1,
                            r,
                            0,
                            E), 0 < b && (C = a.line(e, [0, f, c + f], [0, n, n], r, b, q, D)), H =
                            a.polygon(e, [0, 0, c, c, 0], [0, d, d, 0, 0], B, g, 1, r, 0, E), H.translate(f, n),
                        0 < b && (M = a.line(e, [f, f], [n, n + d], r, b, q, D)), h =
                            a.polygon(e, [0, 0, f, f, 0], [0, d, d + n, n, 0], B, g, 1, r, 0, E), u =
                            a.polygon(e, [c, c, c + f, c + f, c], [0, d, d + n, n, 0], B, g, 1, r, 0, E), 0 < b &&
                            (A = a.line(e, [c, c + f, c + f, c], [0, n, d + n, d], r, b, q, D)), B =
                            a.adjustLuminosity(m, .2), m =
                            a.polygon(e, [0, f, c + f, c, 0], [d, d + n, d + n, d, d], B, g, 1, r, 0, E), 0 < b &&
                            (k = a.line(e, [0, f, c + f], [d, d + n, d + n], r, b, q, D));
                else {
                    var O, P, Q;
                    N
                        ? (O = d / 2, B = f / 2, Q = d / 2, P =
                            c + f / 2, v = Math.abs(d / 2), x = Math.abs(f / 2))
                        : (B = c / 2, O = n / 2, P = c / 2, Q = d + n / 2 + 1, x = Math.abs(c / 2), v =
                            Math.abs(n / 2));
                    J = x * y;
                    L = v * y;
                    .1 < x && .1 < x && (t = a.circle(e, x, G, g, q, r, b, !1, v), t.translate(B, O));
                    .1 < J &&
                        .1 < J &&
                        (m = a.circle(e, J, a.adjustLuminosity(G, .5), g, q, r, b, !1, L), m.translate(P, Q))
                }
            g = K;
            1 > Math.abs(d) && (d = 0);
            1 > Math.abs(c) && (c = 0);
            !isNaN(y) && (0 < Math.abs(f) || 0 < Math.abs(n))
                ? (w = [G], w = { fill: w, stroke: r, "stroke-width": q, "stroke-opacity": b, "fill-opacity": g }, N
                        ? (g = "M0,0 L" + c + "," + (d / 2 - d / 2 * y), q = " B", 0 < c && (q = " A"), a.VML
                            ? (g += q +
                                Math.round(c -
                                    J) +
                                "," +
                                Math.round(d / 2 - L) +
                                "," +
                                Math.round(c + J) +
                                "," +
                                Math.round(d / 2 + L) +
                                "," +
                                c +
                                ",0," +
                                c +
                                "," +
                                d, g = g +
                                (" L0," + d) +
                                (q +
                                    Math.round(-x) +
                                    "," +
                                    Math.round(d / 2 - v) +
                                    "," +
                                    Math.round(x) +
                                    "," +
                                    Math.round(d / 2 + v) +
                                    ",0," +
                                    d +
                                    ",0,0"))
                            : (g += "A" + J + "," + L + ",0,0,0," + c + "," + (d - d / 2 * (1 - y)) + "L0," + d, g +=
                                "A" +
                                x +
                                "," +
                                v +
                                ",0,0,1,0,0"), x = 90)
                        : (q = c / 2 - c / 2 * y, g = "M0,0 L" + q + "," + d, a.VML
                            ? (g = "M0,0 L" + q + "," + d, q =
                                " B", 0 > d && (q = " A"), g += q +
                                Math.round(c / 2 - J) +
                                "," +
                                Math.round(d - L) +
                                "," +
                                Math.round(c / 2 + J) +
                                "," +
                                Math.round(d + L) +
                                ",0," +
                                d +
                                "," +
                                c +
                                "," +
                                d, g += " L" + c + ",0", g += q +
                                Math.round(c /
                                    2 +
                                    x) +
                                "," +
                                Math.round(v) +
                                "," +
                                Math.round(c / 2 - x) +
                                "," +
                                Math.round(-v) +
                                "," +
                                c +
                                ",0,0,0")
                            : (g += "A" + J + "," + L + ",0,0,0," + (c - c / 2 * (1 - y)) + "," + d + "L" + c + ",0",
                                g += "A" +
                                    x +
                                    "," +
                                    v +
                                    ",0,0,1,0,0"), x = 180), e =
                        e.path(g).attr(w),
                    e.gradient("linearGradient", [G, a.adjustLuminosity(G, -.3), a.adjustLuminosity(G, -.3), G], x),
                    N ? e.translate(f / 2, 0) : e.translate(0, n / 2))
                : e = 0 === d
                ? a.line(e, [0, c], [0, 0], r, b, q, D)
                : 0 === c
                ? a.line(e, [0, 0], [0, d], r, b, q, D)
                : 0 < I
                ? a.rect(e, c, d, w, g, q, r, b, I, E, D)
                : a.polygon(e, [0, 0, c, c, 0], [0, d, d, 0, 0], w, g, q, r, b, E, !1, D);
            c = isNaN(y)
                ? 0 > d
                ? [
                    t,
                    C, H, M, h, u, A, m, k, e
                ]
                : [m, k, H, M, h, u, t, C, A, e]
                : N
                ? 0 < c
                ? [t, e, m]
                : [m, e, t]
                : 0 > d
                ? [t, e, m]
                : [m, e, t];
            a.setCN(l, e, z + "front");
            a.setCN(l, H, z + "back");
            a.setCN(l, m, z + "top");
            a.setCN(l, t, z + "bottom");
            a.setCN(l, h, z + "left");
            a.setCN(l, u, z + "right");
            for (t = 0; t < c.length; t++) if (H = c[t]) p.push(H), a.setCN(l, H, z + "element");
            F && e.pattern(F, NaN, l.path)
        },
        width: function(a) {
            isNaN(a) && (a = 0);
            this.w = Math.round(a);
            this.draw()
        },
        height: function(a) {
            isNaN(a) && (a = 0);
            this.h = Math.round(a);
            this.draw()
        },
        animateHeight: function(p, e) {
            var l = this;
            l.easing = e;
            l.totalFrames = Math.round(1E3 * p / a.updateRate);
            l.rh = l.h;
            l.frame = 0;
            l.height(1);
            setTimeout(function() { l.updateHeight.call(l) }, a.updateRate)
        },
        updateHeight: function() {
            var p = this;
            p.frame++;
            var e = p.totalFrames;
            p.frame <= e &&
            (e = p.easing(0, p.frame, 1, p.rh - 1, e), p.height(e), setTimeout(
                function() { p.updateHeight.call(p) },
                a.updateRate))
        },
        animateWidth: function(p, e) {
            var l = this;
            l.easing = e;
            l.totalFrames = Math.round(1E3 * p / a.updateRate);
            l.rw = l.w;
            l.frame = 0;
            l.width(1);
            setTimeout(function() { l.updateWidth.call(l) }, a.updateRate)
        },
        updateWidth: function() {
            var p = this;
            p.frame++;
            var e = p.totalFrames;
            p.frame <= e &&
            (e = p.easing(0, p.frame, 1, p.rw - 1, e), p.width(e), setTimeout(function() { p.updateWidth.call(p) },
                a.updateRate))
        }
    })
})();