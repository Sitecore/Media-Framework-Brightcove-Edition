/*
 * jQuery File Upload User Interface Plugin 8.7.1
 * https://github.com/blueimp/jQuery-File-Upload
 *
 * Copyright 2010, Sebastian Tschan
 * https://blueimp.net
 *
 * Licensed under the MIT license:
 * http://www.opensource.org/licenses/MIT
 */

/*jslint nomen: true, unparam: true, regexp: true */
/*global define, window, URL, webkitURL, FileReader */
 

(function (factory) {
    'use strict';
    if (typeof define === 'function' && define.amd) {
        // Register as an anonymous AMD module:
        define([
            'jquery',
            'tmpl',
            './jquery.fileupload-image',
            './jquery.fileupload-audio',
            './jquery.fileupload-video',
            './jquery.fileupload-validate',
            './jquery.fileupload-ui'
        ], factory);
    } else {
        // Browser globals:
        factory(
            window.jQuery,
            window.tmpl
        );
    }
    
}(function ($, tmpl, loadImage) { 
    'use strict'; 
    var okStatus = { image: "/sitecore/shell/Themes/Standard/Images/MediaFramework/Common/gal.png" };
    var failStatus = { image: "/sitecore/shell/Themes/Standard/Images/MediaFramework/Common/stop.png" };
    var canceledStatus = { image: "/sitecore/shell/Themes/Standard/Images/MediaFramework/Common/stop_sign.png" };
    var buffered = $("#Buffered").val();
    var emptyResult = $("#EmptyResult").val();
    var accountMenuWarn = $("#AccountMenuWarn").val();
    var uploadingCanceled = $("#UploadingCanceled").val();
    var wrongExtension = $('#wrongExtension').val();
    
    var originalStartHandler = $.blueimp.fileupload.prototype._startHandler;
    var originalInitButtonBarEventHandlers = $.blueimp.fileupload.prototype._initButtonBarEventHandlers;
       
    // The UI version extends the file upload widget
    // and adds complete user interface interaction:
    $.widget('blueimp.fileupload', $.blueimp.fileupload, {
        options: {
            // The ID of the accounts list template:
            accountsTemplateId: 'template-accounts',
            uploadStartTemplateId: 'template-upload-start',
            uploadDoneTemplateId: 'template-upload-done',
            accountsContainer: undefined,
            exts: [],
            Accounts: [],

            renderExternalProgress: function(e, data) {
                var that = $(this).data('blueimp-fileupload') ||
                    $(this).data('fileupload');

                var node = data.context.find('.upload-process');
                data.context.accountIds = [];
                $.each(data.context.accounts, function(index) {
                    that._renderUploadStart(data.context.accounts[index], node);
                    data.context.accountIds.push(data.context.accounts[index].id);
                    node.find('.percents').hide();
                    node.find('.external-cancel').hide();
                    node.find('.external-cancel').prop('disabled', true);
                });
            }, 
            
            // Callback for global upload progress events:
            externalProgressAll: function(e, data) {
                var that = $(this).data('blueimp-fileupload') ||
                    $(this).data('fileupload');
                $.each(data.context.progressAccounts, function (index) {
                    var acc = data.context.progressAccounts[index],
                        accId = acc.accountId,
                        progress = acc.progress,
                        error = acc.error,
                        node = data.context.find('div[accid="' + accId + '"]'),
                        label = node.find('.progress-label').html(),
                        mediaItemId = acc.mediaItemId;

                    var status;
                    if (progress == 100) {
                        status = okStatus;
                        status.label = label;
                        status.mode = data.context.mode;
                        status.mediaItemId = mediaItemId;
                        status.database = data.context.database;
                        that._renderUploadDone(status, node);
                        that._cancelAccount(data, accId);
                        
                       
                    } else {
                        if (error != null && error != "") {
                            status = failStatus;
                            status.label = label;
                            status.error = error;
                            that._renderUploadDone(status, node);
                            that._cancelAccount(data, accId);
                        } else {
                            node.find('.external-progress').attr('aria-valuenow', progress).find('.bar').css('width', progress + '%');
                            node.find('.external-progress-percents').html(progress + '%');
                        }
                    }
                });
            },

            buffered: function(e, data) {
                var that = $(this).data('blueimp-fileupload') ||
                    $(this).data('fileupload');
                var status = okStatus;
                status.label = buffered;
                status.mode = 'buffered';
                that._renderUploadDone(status, data.context.find('.buffering'));

                data.context.fileData = data.result;
                data.context.isProcess = true;

                that._onExternalUpload(e, data);

                data.context.find('.external-cancel').prop('disabled', false);
                data.context.find('.external-cancel').fadeIn();
                data.context.find('.cancel').prop('disabled', true);
                data.context.find('.percents').fadeIn(250);
            }
        },

        _cancelAccount: function (data, id) {
            $.each(data.context.accountIds, function(index) {
                if (data.context.accountIds[index] == id) {
                    data.context.accountIds.splice(index, 1);
                }
            });
            if (data.context.accountIds.length == 0) {
                data.context.isProcess = false;
            }
        },

        _onExternalUpload: function(e, data) {
            var that = this,
                result = data.result || { Error: emptyResult },
                now = new Date();
             
            $.ajax({
                url: '../../../../../sitecore modules/Web/MediaFramework/Upload/MediaUpload.ashx?rt=pr&id=' + result.id + '&accounts=' + data.context.accountIds + '&d=' + now,
                success: function(res) {
                    var responce;
                    if (data.context.isProcess == false) {
                        data.context.find('.buffering').hide(500);
                    } else {
                        if (res != 'null' && (responce = JSON.parse(res)) != null) {
                            data.context.progressAccounts = responce.progressList;
                            that._trigger('externalProgressAll', e, data); 
                        }

                        window.setTimeout(function() {
                            that._onExternalUpload(e, data);
                        }, 5000);
                    }
                }
            });
        },

        _startHandler: function (e) { 
       
            if (!this._hasAccount()) {
                e.preventDefault(); 
                $('#errorLabel').hide().html(accountMenuWarn).fadeIn(250);
            } else {
                $('#errorLabel').html('');
                originalStartHandler.call(this, e);
            }
        },

        _onSend: function(e, data) {

            var url = '',
                sep,
                accounts = [];

            var that = this;
            $('.account-box').each(function() {
                var box = $(this);
                if (box.prop('checked')) {
                    sep = url.length > 0 ? ',' : '';
                   
                    var cAcc = that._findAccountbyId(box.attr('accId'));
                    if (cAcc) { 
                        url += sep + cAcc.id + '|' + cAcc.accountTemplateId;
                        accounts.push(cAcc);
                    }
                }
            });

            if (this._ValidateFileExtension(accounts, data)) {
                return false;
            }

            var settings = this._pageData();
            $('#fileupload').fileupload({
                // Uncomment the following to send cross-domain cookies:
                //xhrFields: {withCredentials: true},
                url: '../../../../../sitecore modules/Web/MediaFramework/Upload/MediaUpload.ashx?rt=up&accounts=' + url + '&db=' + settings.database + '&m=' + settings.mode
            });
            
            data.context.database = settings.database;
            data.context.mode = settings.mode;
            data.context.accounts = accounts;
            
            this._trigger('renderExternalProgress', e, data);

            if (!data.submit) {
                this._addConvenienceMethods(e, data);
            }
            var jqXHR,
                aborted,
                slot,
                pipe,
                options = that._getAJAXSettings(data),
                send = function() {
                    that._sending += 1;
                    // Set timer for bitrate progress calculation:
                    options._bitrateTimer = new that._BitrateTimer();
                    jqXHR = jqXHR || (
                        ((aborted || that._trigger('send', e, options) === false) &&
                            that._getXHRPromise(false, options.context, aborted)) ||
                            that._chunkedUpload(options) || $.ajax(options)
                    ).done(function(result, textStatus, jqXHR) {
                        that._onDone(result, textStatus, jqXHR, options);
                    }).fail(function(jqXHR, textStatus, errorThrown) {
                        that._onFail(jqXHR, textStatus, errorThrown, options);
                    }).always(function(jqXHRorResult, textStatus, jqXHRorError) {
                        that._onAlways(
                            jqXHRorResult,
                            textStatus,
                            jqXHRorError,
                            options
                        );
                        that._sending -= 1;
                        that._active -= 1;
                        if (options.limitConcurrentUploads &&
                            options.limitConcurrentUploads > that._sending) {
                            // Start the next queued upload,
                            // that has not been aborted:
                            var nextSlot = that._slots.shift();
                            while (nextSlot) {
                                if (that._getDeferredState(nextSlot) === 'pending') {
                                    nextSlot.resolve();
                                    break;
                                }
                                nextSlot = that._slots.shift();
                            }
                        }
                        if (that._active === 0) {
                            // The stop callback is triggered when all uploads have
                            // been completed, equivalent to the global ajaxStop event:
                            that._trigger('stop');
                        }
                    });
                    return jqXHR;
                };
            this._beforeSend(e, options);
            if (this.options.sequentialUploads ||
                (this.options.limitConcurrentUploads &&
                    this.options.limitConcurrentUploads <= this._sending)) {
                if (this.options.limitConcurrentUploads > 1) {
                    slot = $.Deferred();
                    this._slots.push(slot);
                    pipe = slot.pipe(send);
                } else {
                    this._sequence = this._sequence.pipe(send, send);
                    pipe = this._sequence;
                }
                // Return the piped Promise object, enhanced with an abort method,
                // which is delegated to the jqXHR object of the current upload,
                // and jqXHR callbacks mapped to the equivalent Promise methods:
                pipe.abort = function() {
                    aborted = [undefined, 'abort', 'abort'];
                    if (!jqXHR) {
                        if (slot) {
                            slot.rejectWith(options.context, aborted);
                        }
                        return send();
                    }
                    return jqXHR.abort();
                };
                return this._enhancePromise(pipe);
            }
            return send();


            //  originalOnSend.call(this, e, data);
        },

        _findAccountbyId: function(id) {

            var current = {};
            _.each(this.options.Accounts, function(ac) {
                var c = _.findWhere(ac, { id: id }); 
                if (c) {
                    current = c;
                    return;
                }
            });

            return current;
        },

        _ValidateFileExtension: function(accounts, data) {
            var that = this,
                fileName = data.files[0].name.replace(/^.*[\\\/]/, ''), 
                fileNameParts = fileName ? fileName.split('.') : [],
                ext = fileNameParts.length == 0 ? '' : fileNameParts[fileNameParts.length - 1],
                mess = wrongExtension,
                errorMessage = '',
                extensions = [];
            _.each(accounts, function (ac) {
                var exts = ac.exts;
                _.each(exts, function (e) {
                    if (!that._ArrayContains(extensions, e)) {
                        extensions.push(e);
                    }
                });
            });
            if (!that._ArrayContains(extensions, ext)) {
                errorMessage = fileName + ' ' + mess + ' ' + extensions;
                that._renderUploadCanceled(data.context.find('.buffering'), data, errorMessage, fileName);
            }
            
            return errorMessage;
        },

        _onDone: function(result, textStatus, jqXHR, options) {
            var total = options._progress.total,
                response = options._response;
            if (options._progress.loaded < total) {
                // Create a progress event if no final progress event
                // with loaded equaling total has been triggered:
                this._onProgress($.Event('progress', {
                    lengthComputable: true,
                    loaded: total,
                    total: total
                }), options);
            }
            response.result = options.result = result;
            response.textStatus = options.textStatus = textStatus;
            response.jqXHR = options.jqXHR = jqXHR;
            this._trigger('buffered', null, options);
        },
        
        // My method for generating account list
        _initControls: function() {
            var page = this._pageData();
            this._renderAccounts(this.options.accountsTemplate, page);
        },

        _ArrayContains: function(a, obj) {
            for (var i = 0; i < a.length; i++) {
                if (a[i] === obj) {
                    return true;
                }
            }
            return false;
        },

        _initButtonBarEventHandlers: function() {
            originalInitButtonBarEventHandlers.call(this);
            this._initExternalEventHandlers();
        },

        _initExternalEventHandlers: function() {

            var fileUploadButtonBar = this.element.find('.fileupload-buttonbar'),
                filesList = this.options.filesContainer;

            this._on(fileUploadButtonBar.find('.cancel'), {
                click: function(e) {
                    e.preventDefault();
                    filesList.find('.external-cancel').click();
                }
            });

            this._on(filesList, {
                'click .external-cancel': this._externalCancelHandler
            });

            this._on(filesList, {
                'click .goto': this._goToItemHandler
            });
        },

        _getFrame: function() {
            return $(window.parent.parent.document).find("iframe[src^='/sitecore/shell/Applications/Content%20Editor.aspx'], iframe[src^='/sitecore/shell/Applications/Content Editor.aspx']").get(0);
        },

        _goToItemHandler: function(e) {
            var mode = $(e.target).attr('mode'),
                db = $(e.target).attr('database'),
                mItemId = $(e.target).attr('mItemId'),
                that = this;
            
            if (mode == "norm") {
                var frame = this._getFrame();
                if (!frame || !frame.contentWindow.scForm) {
                    var win = parent.parent.window.open("/sitecore/shell/Applications/Content%20editor.aspx?fo=%7b" + mItemId + "%7d&amp;la=en&amp;vs=1&amp;we=1");
                    win.focus();
                   
                } else {
                    frame.contentWindow.scForm.postRequest('', '', '', 'item:load(id={' + mItemId + '}, db=' + db + ')');
                }

            } else {
                var input = $(window.parent.document).find("#Filename");
                input.val(mItemId);
            }
            return false;
        },

        _externalCancelHandler: function(e) {
            var accId = $(e.target).attr('accid');
            var template = $(e.currentTarget).closest('.template-upload'),
                data = template.data('data') || {};
            var that = this;

            var account = _.findWhere(data.context.accounts, { id: accId });
            
            $.ajax({
                url: '../../../../../sitecore modules/Web/MediaFramework/Upload/MediaUpload.ashx?rt=cncl&fid=' + data.context.fileData.id + '&accId=' + accId + '&accTemplateId=' + account.accountTemplateId,
                success: function (res) {
                    var node = data.context.find('div[accid="' + accId + '"]'),
                        label = node.find('.progress-label').html();
                    
                    //if (res == 0) {
                    //    var status = okStatus;
                    //    status.label = label;
                    //    status.mode = data.context.mode;
                    //    status.mediaItemId = mediaItemId;
                    //    status.database = data.context.database;
                    //    status.text = data.context.mode == 'norm' ? 'GO TO ITEM' : 'SELECT ITEM';
                        
                    //    that._cancelAccount(data, accId);
                    //    that._renderUploadCanceled(node, data, "Uploading is canceled!", label);
                    //} else {
                        
                        that._cancelAccount(data, accId);
                        that._renderUploadCanceled(node, data, uploadingCanceled, label);
                   // } 
                }
            });
            return false;
        },

        _renderUploadCanceled: function (node, data, error, name) { 
           var status = {
               image: "/sitecore/shell/Themes/Standard/Images/MediaFramework/Common/stop_sign.png",
                label: name,
                error: error,
                mode: 'buffered'
            };
            this._renderUploadDone(status, node); 
        },
        
        _hasAccount: function () {
            var ok = false;
            $('.account-box').each(function () {
                var box = $(this);
                if (box.prop('checked')) {
                    ok = true;
                } 
            });
             
            return ok;
        },

        // returns server hiden field data in json format
        _pageData: function () { 
            return JSON.parse($("#PageData").val());
        },

        _renderAccounts: function (func, page) {
            this._selectAccount(page);
            
            var result = func({
                accounts: page.allAccounts,
                options: this.options
            });

            this.options.Accounts = page.allAccounts;
            return this._renderMFTemplates(result, this.options.accountsContainer);
        },
        
        _selectAccount: function (page) {
            if (page.allAccounts.length == 1) {
                var accList = page.allAccounts[0];
                if (accList.length == 1) {
                    accList[0].selected = true;
                }
            }
        },
        
        _renderUploadDone: function (uploadStatus, container) {
  
            var func = this.options.uploadDoneTemplate;
            var result = func({
                status: uploadStatus,
                options: this.options 
            });

            return this._renderMFTemplates(result, container);
        }, 

        _renderUploadStart: function (accountObj, container) {
            var func = this.options.uploadStartTemplate;
             
            var result = func({
                account: accountObj,
                options:this.options
            });

            return this._appendTemplates(result, container);
        },
        
        _appendTemplates: function (result, container) {
            if (result instanceof $) {
                return result;
            }
            
            return container.append(result).children();
        },
        
        _renderMFTemplates: function (result, container) {
            if (result instanceof $) {
                return result;
            }
            return $(container).html(result).children();
        },
         
        _initSpecialOptions: function () {
            this._super();
            this._initFilesContainer();
            this._initTemplates();
            this._initControls();
        },
        
        _initTemplates: function () {
             
            var options = this.options;
            options.templatesContainer = this.document[0].createElement(options.filesContainer.prop('nodeName'));
            if (tmpl) {
                options.accountsContainer = $('.fileupload-accounts');

                if (options.accountsTemplateId) {
                    options.accountsTemplate = tmpl(options.accountsTemplateId);
                }

                if (options.uploadDoneTemplateId) {
                    options.uploadDoneTemplate = tmpl(options.uploadDoneTemplateId);
                }

                if (options.uploadStartTemplateId) {
                    options.uploadStartTemplate = tmpl(options.uploadStartTemplateId);
                }


                if (options.uploadTemplateId) {
                    options.uploadTemplate = tmpl(options.uploadTemplateId);
                } 
            }
        }
    });
}));
