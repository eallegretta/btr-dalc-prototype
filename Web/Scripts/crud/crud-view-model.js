var CrudViewModel = (function () {
    var _self = this,
		_cfg,
		_currentDiv,
		_currentFilters = "",
		_currentPage = 1;

    _self.create = function () {
        location.hash = "create";
    };


    _self.filter = function () {
        _currentFilters = $("#filters_form").serialize();

        var hash = "#page/" + _currentPage + "/" + _currentFilters;

        location.hash = hash;
    };

    _self.clearFilters = function () {
        $("#filters_form")[0].reset();

        if (_currentPage == 1 && _currentFilters == "")
            return;

        _currentPage = 1;
        _currentFilters = "";

        location.hash = "";

        list();
    };

    function selectFilterValues() {
        foreachCurrentFilter(function (key, value) {
            var elem = $("[name='" + key + "']");
            if (elem.val() != value) {
                elem.val(value);
            }
        });
    }


    function getDiv(className) {
        var div = $("." + className);
        if (div.length == 0) {
            div = $("<div />").addClass(className);
            div.appendTo(document);
        }
        return div;
    }

    function attachAjaxForm(container) {
        container.find("form").ajaxForm({
            target: container,
            success: function () {
                container.find(".dialog-title").remove();
                attachAjaxForm(container);
            }
        });
    }

    function loadPartialView(container, url, params) {
        var div = _currentDiv = getDiv(container);
        $.get(url, params, function (data) {
            div.html(data);
            div.dialog({
                close: function (event, ui) {
                    location.hash = "";
                    _currentDiv = null;
                },
                modal: true
            });
            var dialogTitle = div.find(".dialog-title");
            div.dialog("option", "title", dialogTitle.html());
            attachAjaxForm(div);
            dialogTitle.remove();

            createJQueryUIButtons(div);
        });
    }

    function foreachCurrentFilter(fnc) {
        var filterParts = _currentFilters.split("&");
        $(filterParts).each(function () {
            var filter = this.split("=");
            if (fnc) {
                fnc(filter[0], filter[1]);
            }
        });
    }


    function list() {
        var params = { page: _currentPage };

        foreachCurrentFilter(function (key, value) {
            params[key] = value;
        });

        $(".crud-list").load(_cfg.listUrl, params);
    };

    function create() {
        loadPartialView("crud-create", _cfg.createUrl);
    }

    function details(id) {
        loadPartialView("crud-details", _cfg.detailsUrl, { id: id });
    }

    function edit(id) {
        loadPartialView("crud-edit", _cfg.editUrl, { id: id });
    }

    function deleteItem(id) {
        loadPartialView("crud-delete", _cfg.deleteUrl, { id: id });
    }

    function addCustomAction(containerId, route, url, getParamsFunc) {
        routes.get(route, function () {
            var params = null;
            if (getParamsFunc) {
                params = getParamsFunc(this.params);
            }
            else {
                params = {};
                for (var prop in this.params) {
                    if (this.params.hasOwnProperty(prop)) {
                        params[prop] = this.params[prop];
                    }
                }
            }

            loadPartialView(containerId, url, params);
        });
    }

    var routes = Sammy();


    function createRoutes(customActions) {
       

        routes
			.get('#page/:pageNumber', function () {
			    _currentPage = this.params.pageNumber;

			    if (!_currentFilters) {
			        list();
			    }
			    else {
			        location.hash = "#page/" + _currentPage + "/" + _currentFilters;
			    }
			})
			.get('#page/:pageNumber/:filters', function () {
			    _currentPage = this.params.pageNumber;
			    _currentFilters = this.params.filters;

			    list();

			    selectFilterValues();
			})
			.get('', function () {
			});

        addCustomAction("crud-create", "#create", _cfg.createUrl);
        addCustomAction("crud-details", "#details/:id", _cfg.detailsUrl);
        addCustomAction("crud-edit", "#edit/:id", _cfg.editUrl);
        addCustomAction("crud-delete", "#delete/:id", _cfg.deleteUrl);

        $(customActions).each(function () {
            addCustomAction(this.containerId, this.route, this.url, this.getParams);
        });


        routes.run();
    }

    function createJQueryUIButtons(container) {
        container = container || $(".crud");

        container.find("input:submit, input:reset, button, input:button").button();
    }

    $(document).ready(function () {
        createJQueryUIButtons();
    });

    return {
        configure: function (cfg) {
            _cfg = cfg;

            createRoutes(cfg.customActions);

        },

        getInstance: function () {
            return _self;
        },

        closeOpenedDialog: function () {
            if (_currentDiv) {
                _currentDiv.dialog("close");
            }
        },

        addCustomAction: addCustomAction

    };
})();


ko.applyBindings(CrudViewModel.getInstance());