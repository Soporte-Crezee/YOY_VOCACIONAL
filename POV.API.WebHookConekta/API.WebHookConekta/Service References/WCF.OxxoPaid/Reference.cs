﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace API.WebHookConekta.WCF.OxxoPaid {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="WCF.OxxoPaid.IOxxoPaidService")]
    public interface IOxxoPaidService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOxxoPaidService/UpdateAspirantes", ReplyAction="http://tempuri.org/IOxxoPaidService/UpdateAspirantesResponse")]
        string UpdateAspirantes(string id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOxxoPaidService/UpdateAspirantes", ReplyAction="http://tempuri.org/IOxxoPaidService/UpdateAspirantesResponse")]
        System.Threading.Tasks.Task<string> UpdateAspirantesAsync(string id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOxxoPaidService/RetrieveAspirante", ReplyAction="http://tempuri.org/IOxxoPaidService/RetrieveAspiranteResponse")]
        string RetrieveAspirante(string id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOxxoPaidService/RetrieveAspirante", ReplyAction="http://tempuri.org/IOxxoPaidService/RetrieveAspiranteResponse")]
        System.Threading.Tasks.Task<string> RetrieveAspiranteAsync(string id);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IOxxoPaidServiceChannel : API.WebHookConekta.WCF.OxxoPaid.IOxxoPaidService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class OxxoPaidServiceClient : System.ServiceModel.ClientBase<API.WebHookConekta.WCF.OxxoPaid.IOxxoPaidService>, API.WebHookConekta.WCF.OxxoPaid.IOxxoPaidService {
        
        public OxxoPaidServiceClient() {
        }
        
        public OxxoPaidServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public OxxoPaidServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public OxxoPaidServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public OxxoPaidServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string UpdateAspirantes(string id) {
            return base.Channel.UpdateAspirantes(id);
        }
        
        public System.Threading.Tasks.Task<string> UpdateAspirantesAsync(string id) {
            return base.Channel.UpdateAspirantesAsync(id);
        }
        
        public string RetrieveAspirante(string id) {
            return base.Channel.RetrieveAspirante(id);
        }
        
        public System.Threading.Tasks.Task<string> RetrieveAspiranteAsync(string id) {
            return base.Channel.RetrieveAspiranteAsync(id);
        }
    }
}
