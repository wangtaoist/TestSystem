﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// 此源代码是由 Microsoft.VSDesigner 4.0.30319.42000 版自动生成。
// 
#pragma warning disable 1591

namespace TestDAL.WebReference {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="WebService1Soap", Namespace="LCHSE")]
    public partial class WebService1 : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback HelloWorldOperationCompleted;
        
        private System.Threading.SendOrPostCallback jfOperationCompleted;
        
        private System.Threading.SendOrPostCallback SnCxOperationCompleted;
        
        private System.Threading.SendOrPostCallback SnCx_snOperationCompleted;
        
        private System.Threading.SendOrPostCallback SnCx_LYOperationCompleted;
        
        private System.Threading.SendOrPostCallback SnCx_SCOperationCompleted;
        
        private System.Threading.SendOrPostCallback SnCx_BZLYOperationCompleted;
        
        private System.Threading.SendOrPostCallback SnCx_BZSNOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public WebService1() {
            this.Url = global::TestDAL.Properties.Settings.Default.TestDAL_WebReference_WebService1;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event HelloWorldCompletedEventHandler HelloWorldCompleted;
        
        /// <remarks/>
        public event jfCompletedEventHandler jfCompleted;
        
        /// <remarks/>
        public event SnCxCompletedEventHandler SnCxCompleted;
        
        /// <remarks/>
        public event SnCx_snCompletedEventHandler SnCx_snCompleted;
        
        /// <remarks/>
        public event SnCx_LYCompletedEventHandler SnCx_LYCompleted;
        
        /// <remarks/>
        public event SnCx_SCCompletedEventHandler SnCx_SCCompleted;
        
        /// <remarks/>
        public event SnCx_BZLYCompletedEventHandler SnCx_BZLYCompleted;
        
        /// <remarks/>
        public event SnCx_BZSNCompletedEventHandler SnCx_BZSNCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("LCHSE/HelloWorld", RequestNamespace="LCHSE", ResponseNamespace="LCHSE", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string HelloWorld() {
            object[] results = this.Invoke("HelloWorld", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void HelloWorldAsync() {
            this.HelloWorldAsync(null);
        }
        
        /// <remarks/>
        public void HelloWorldAsync(object userState) {
            if ((this.HelloWorldOperationCompleted == null)) {
                this.HelloWorldOperationCompleted = new System.Threading.SendOrPostCallback(this.OnHelloWorldOperationCompleted);
            }
            this.InvokeAsync("HelloWorld", new object[0], this.HelloWorldOperationCompleted, userState);
        }
        
        private void OnHelloWorldOperationCompleted(object arg) {
            if ((this.HelloWorldCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.HelloWorldCompleted(this, new HelloWorldCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("LCHSE/jf", RequestNamespace="LCHSE", ResponseNamespace="LCHSE", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int jf(string a, string b) {
            object[] results = this.Invoke("jf", new object[] {
                        a,
                        b});
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void jfAsync(string a, string b) {
            this.jfAsync(a, b, null);
        }
        
        /// <remarks/>
        public void jfAsync(string a, string b, object userState) {
            if ((this.jfOperationCompleted == null)) {
                this.jfOperationCompleted = new System.Threading.SendOrPostCallback(this.OnjfOperationCompleted);
            }
            this.InvokeAsync("jf", new object[] {
                        a,
                        b}, this.jfOperationCompleted, userState);
        }
        
        private void OnjfOperationCompleted(object arg) {
            if ((this.jfCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.jfCompleted(this, new jfCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("LCHSE/SnCx", RequestNamespace="LCHSE", ResponseNamespace="LCHSE", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string SnCx(string a, string b) {
            object[] results = this.Invoke("SnCx", new object[] {
                        a,
                        b});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void SnCxAsync(string a, string b) {
            this.SnCxAsync(a, b, null);
        }
        
        /// <remarks/>
        public void SnCxAsync(string a, string b, object userState) {
            if ((this.SnCxOperationCompleted == null)) {
                this.SnCxOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSnCxOperationCompleted);
            }
            this.InvokeAsync("SnCx", new object[] {
                        a,
                        b}, this.SnCxOperationCompleted, userState);
        }
        
        private void OnSnCxOperationCompleted(object arg) {
            if ((this.SnCxCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SnCxCompleted(this, new SnCxCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("LCHSE/SnCx_sn", RequestNamespace="LCHSE", ResponseNamespace="LCHSE", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string SnCx_sn(string a) {
            object[] results = this.Invoke("SnCx_sn", new object[] {
                        a});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void SnCx_snAsync(string a) {
            this.SnCx_snAsync(a, null);
        }
        
        /// <remarks/>
        public void SnCx_snAsync(string a, object userState) {
            if ((this.SnCx_snOperationCompleted == null)) {
                this.SnCx_snOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSnCx_snOperationCompleted);
            }
            this.InvokeAsync("SnCx_sn", new object[] {
                        a}, this.SnCx_snOperationCompleted, userState);
        }
        
        private void OnSnCx_snOperationCompleted(object arg) {
            if ((this.SnCx_snCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SnCx_snCompleted(this, new SnCx_snCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("LCHSE/SnCx_LY", RequestNamespace="LCHSE", ResponseNamespace="LCHSE", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string SnCx_LY(string a) {
            object[] results = this.Invoke("SnCx_LY", new object[] {
                        a});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void SnCx_LYAsync(string a) {
            this.SnCx_LYAsync(a, null);
        }
        
        /// <remarks/>
        public void SnCx_LYAsync(string a, object userState) {
            if ((this.SnCx_LYOperationCompleted == null)) {
                this.SnCx_LYOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSnCx_LYOperationCompleted);
            }
            this.InvokeAsync("SnCx_LY", new object[] {
                        a}, this.SnCx_LYOperationCompleted, userState);
        }
        
        private void OnSnCx_LYOperationCompleted(object arg) {
            if ((this.SnCx_LYCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SnCx_LYCompleted(this, new SnCx_LYCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("LCHSE/SnCx_SC", RequestNamespace="LCHSE", ResponseNamespace="LCHSE", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string SnCx_SC(string a, string b) {
            object[] results = this.Invoke("SnCx_SC", new object[] {
                        a,
                        b});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void SnCx_SCAsync(string a, string b) {
            this.SnCx_SCAsync(a, b, null);
        }
        
        /// <remarks/>
        public void SnCx_SCAsync(string a, string b, object userState) {
            if ((this.SnCx_SCOperationCompleted == null)) {
                this.SnCx_SCOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSnCx_SCOperationCompleted);
            }
            this.InvokeAsync("SnCx_SC", new object[] {
                        a,
                        b}, this.SnCx_SCOperationCompleted, userState);
        }
        
        private void OnSnCx_SCOperationCompleted(object arg) {
            if ((this.SnCx_SCCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SnCx_SCCompleted(this, new SnCx_SCCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("LCHSE/SnCx_BZLY", RequestNamespace="LCHSE", ResponseNamespace="LCHSE", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string SnCx_BZLY(string a) {
            object[] results = this.Invoke("SnCx_BZLY", new object[] {
                        a});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void SnCx_BZLYAsync(string a) {
            this.SnCx_BZLYAsync(a, null);
        }
        
        /// <remarks/>
        public void SnCx_BZLYAsync(string a, object userState) {
            if ((this.SnCx_BZLYOperationCompleted == null)) {
                this.SnCx_BZLYOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSnCx_BZLYOperationCompleted);
            }
            this.InvokeAsync("SnCx_BZLY", new object[] {
                        a}, this.SnCx_BZLYOperationCompleted, userState);
        }
        
        private void OnSnCx_BZLYOperationCompleted(object arg) {
            if ((this.SnCx_BZLYCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SnCx_BZLYCompleted(this, new SnCx_BZLYCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("LCHSE/SnCx_BZSN", RequestNamespace="LCHSE", ResponseNamespace="LCHSE", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string SnCx_BZSN(string a) {
            object[] results = this.Invoke("SnCx_BZSN", new object[] {
                        a});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void SnCx_BZSNAsync(string a) {
            this.SnCx_BZSNAsync(a, null);
        }
        
        /// <remarks/>
        public void SnCx_BZSNAsync(string a, object userState) {
            if ((this.SnCx_BZSNOperationCompleted == null)) {
                this.SnCx_BZSNOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSnCx_BZSNOperationCompleted);
            }
            this.InvokeAsync("SnCx_BZSN", new object[] {
                        a}, this.SnCx_BZSNOperationCompleted, userState);
        }
        
        private void OnSnCx_BZSNOperationCompleted(object arg) {
            if ((this.SnCx_BZSNCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SnCx_BZSNCompleted(this, new SnCx_BZSNCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    public delegate void HelloWorldCompletedEventHandler(object sender, HelloWorldCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class HelloWorldCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal HelloWorldCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    public delegate void jfCompletedEventHandler(object sender, jfCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class jfCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal jfCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public int Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    public delegate void SnCxCompletedEventHandler(object sender, SnCxCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SnCxCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SnCxCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    public delegate void SnCx_snCompletedEventHandler(object sender, SnCx_snCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SnCx_snCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SnCx_snCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    public delegate void SnCx_LYCompletedEventHandler(object sender, SnCx_LYCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SnCx_LYCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SnCx_LYCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    public delegate void SnCx_SCCompletedEventHandler(object sender, SnCx_SCCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SnCx_SCCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SnCx_SCCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    public delegate void SnCx_BZLYCompletedEventHandler(object sender, SnCx_BZLYCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SnCx_BZLYCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SnCx_BZLYCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    public delegate void SnCx_BZSNCompletedEventHandler(object sender, SnCx_BZSNCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SnCx_BZSNCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SnCx_BZSNCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591