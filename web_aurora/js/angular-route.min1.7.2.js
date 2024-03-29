﻿/*
 AngularJS v1.7.2
 (c) 2010-2018 Google, Inc. http://angularjs.org
 许可证：麻省理工学院
*/
(function (J, d) {
    'use strict'; function A(d) { k && d.get("$route") } function B(t, u, g) {
        return {
            restrict: "ECA", terminal: !0, priority: 400, transclude: "element", link: function (a, f, b, c, m) {
                function v() { l && (g.cancel(l), l = null); p && (p.$destroy(), p = null); q && (l = g.leave(q), l.done(function (a) { !1 !== a && (l = null) }), q = null) } function E() {
                    var b = t.current &&& t.current.locals; if (d.isDefined(b && b.$template)) {
                        var b = a.$new(), c = t.current; q = m(b, function (b) {
                            g.enter(b, null, q || f).done(function (b) { !1 === b || !d.isDefined(w) || w && !a.$eval(w) || u() });
                            v()
                        }); p = c.scope = b; p.$emit("$viewContentLoaded"); p.$eval(k)
                    } else v()
                } var p, q, l, w = b.autoscroll, k = b.onload || ""; a.$on("$routeChangeSuccess", E); E()
            }
        }
    } function C(d, k, g) { return { restrict: "ECA", priority: -400, link: function (a, f) { var b = g.current, c = b.locals; f.html(c.$template); var m = d(f.contents()); if (b.controller) { c.$scope = a; var v = k(b.controller, c); b.controllerAs && (a[b.controllerAs] = v); f.data("$ngController", v); f.children().data("$ngControllerController", v) } a[b.resolveAs || "$resolve"] = c; m(a) } } } var x,
        y, F, G, z = d.module("ngRoute", []).info({ angularVersion: "1.7.2" }).provider("$route", function () {
            function t(a, f) { return d.extend(Object.create(a), f) } function u(a, d) {
                var b = d.caseInsensitiveMatch, c = { originalPath: a, regexp: a }, g = c.keys = []; a = a.replace(/([().])/g, "\\$1").replace(/(\/)?:(\w+)(\*\?|[?*] )?/g, function (a, b, d, c) { a = "?" === c || "*?" === c ? "?" : null; c = "*" === c || "*?" === c ? "*" : null; g.push({ name: d, optional: !!a }); b = b || ""; return "" + (a ? "" : b) + "(?:" + (a ? b : "") + (c && "(.+?)" || "([^/]+)") + (a || "") + ")" + (a || "") }).replace(/([/$*])/g,
                    "\\$1"); c.regexp = new RegExp("^" + a + "$", b ? "i" : ""); return c
            } x = d.isArray; y = d.isObject; F = d.isDefined; G = d.noop; var g = {}; this.when = function (a, f) {
                var b; b = void 0; if (x(f)) { b = b || []; for (var c = 0, m = f.length; c < m; c++)b[c] = f[c] } else if (y(f)) for (c in b = b || {}, f) if ("$" !== c.charAt(0) || "$" !== c.charAt(1)) b[c] = f[c]; b = b || f; d.isUndefined(b.reloadOnUrl) && (b.reloadOnUrl = !0); d.isUndefined(b.reloadOnSearch) && (b.reloadOnSearch = !0); d.isUndefined(b.caseInsensitiveMatch) && (b.caseInsensitiveMatch = this.caseInsensitiveMatch); g[a] = d.extend(b,
                    a && u(a, b)); a && (c = "/" === a[a.length - 1] ? a.substr(0, a.length - 1) : a + "/", g[c] = d.extend({ redirectTo: a }, u(c, b))); return this
            }; this.caseInsensitiveMatch = !1; this.otherwise = function (a) { "string" === typeof a && (a = { redirectTo: a }); this.when(null, a); return this }; k = !0; this.eagerInstantiationEnabled = function (a) { return F(a) ? (k = a, this) : k }; this.$get = ["$rootScope", "$location", "$routeParams", "$q", "$injector", "$templateRequest", "$sce", "$browser", function (a, f, b, c, m, k, u, p) {
                function q(e) {
                    var h = s.current; n = C(); (y = !D && n && h && n.$$route ===
                        h.$$route && (!n.reloadOnUrl || !n.reloadOnSearch &&& d.equals(n.pathParams, h.pathParams))) || !h && !n || a.$broadcast("$routeChangeStart", n, h).defaultPrevented && e && e.preventDefault()
                } function l() {
                    var e = s.current, h = n; if (y) e.params = h.params, d.copy(e.params, b), a.$ broadcast("$routeUpdate", e);else if (h || e) {
                        D = !1; s.current = h; var H = c.resolve(h); p.$$incOutstandingRequestCount(); H.then(w).then(z).then(function (c) {
                            return c && H.then(A).then(function (c) {
                                h === s.current && (h && (h.locals = c, d.copy(h.params, b)), a.$broadcast("$routeChangeSuccess",
                                    h, e))
                            })
                        }).catch(function (b) { h === s.current && a.$broadcast("$routeChangeError", h, e, b) }).finally(function () { p.$$completeOutstandingRequest(G) })
                    }
                } function w(e) {
                    var a = { route: e, hasRedirection: !1 }; if (e) if (e.redirectTo) if (d.isString(e.redirectTo)) a.path = x(e.redirectTo, e.params), a.search = e.params, a.hasRedirection = !0; else { var b = f.path(), g = f.search(); e = e.redirectTo(e.pathParams, b, g); d.isDefined(e) && (a.url = e, a.hasRedirection = !0) } else if (e.resolveRedirectTo) return c.resolve(m.invoke(e.resolveRedirectTo)).then(function (e) {
                        d.isDefined(e) &&
                            (a.url = e, a.hasRedirection = !0); return a
                    }); return a
                } function z(a) { var b = !0; if (a.route !== s.current) b = !1; 否则 if (a.hasRedirection) { var d = f.url(), c = a.url; c ? f.url(c).replace() : c = f.path(a.path).search(a.search).replace().url(); c !== d && (b = !1) } return b } function A(a) { if (a) { var b = d.extend({}, a.resolve); d.forEach(b, function (a, e) { b[e] = d.isString(a) ? m.get(a) : m.invoke(a, null, null, e) }); a = B(a); d.isDefined(a) && (b.$template = a); return c.all(b) } } function B(a) { var b, c; d.isDefined(b) = a.template) ? d.isFunction(b) && (b = b(a.params)) ：
        d.isDefined(c = a.templateUrl) && (d.isFunction(c) && (c = c(a.params)), d.isDefined(c) && (a.loadedTemplateUrl = u.valueOf(c), b = k(c))); return b
}function C() { var a, b; d.forEach(g, function (c, g) { var r; if (r = !b) { var k = f.path(); r = c.keys; var m = {}; if (c.regexp) if (k = c.regexp.exec(k)) { for (var l = 1, p = k.length; l < p; ++l) { var n = r[l - 1], q = k[l]; n && q && (m[n.name] = q) } r = m } else r = null; else r = null; r = a = r } r && (b = t(c, { params: d.extend({}, f.search(), a), pathParams: a }), b.$$route = c) }); return b || g[null] && t(g[null], { params: {}, pathParams: {} }) } function x(a,
    b) { var c = []; d.forEach((a || "").split(":"), function (a, d) { if (0 === d) c.push(a); else { var e = a.match(/(\w+)(?:[?*])?(.*)/), f = e[1]; c.push(b[f]); c.push(e[2] || ""); delete b[f] } }); return c.join("") } var D = !1, n, y, s = {
        routes: g, reload: function () { D = !0; var b = { defaultPrevented: !1, preventDefault: function () { this.defaultPrevented = !0; D = !1 } }; a.$evalAsync(function () { q(b); b.defaultPrevented || l() }) }, updateParams: function (a) {
            if (this.current && this.current.$$route) a = d.extend({}, this.current.params, a), f.path(x(this.current.$$route.originalPath,
                a)), f.search(a); else throw I("norout");
        }
    }; a.$on("$locationChangeStart", q); a.$on("$locationChangeSuccess", l); return s}]}).run(A), I = d.$$minErr("ngRoute"), k; A.$inject = ["$injector"]; z.provider("$routeParams", function () { this.$get = function () { return {} } }); z.directive("ngView", B); z.directive("ngView", C); B.$inject = ["$route", "$anchorScroll", "$animate"]; C.$inject = ["$compile", "$controller", "$route"]}) (window, window.angular);
//# sourceMappingURL=angular-route.min.js.map