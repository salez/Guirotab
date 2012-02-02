<%@ Page Language="C#" ContentType="text/css" Inherits="System.Web.Mvc.ViewPage" %>
.formError .formErrorContent
{
    background: <%=Util.Configuracao.AppSettings("VECorFundo") %>;
    color: <%=Util.Configuracao.AppSettings("VECorFonte") %>;
    width: <%=Util.Configuracao.AppSettings("VELargura") %>;
    box-shadow: 0px 0px <%=Util.Configuracao.AppSettings("VETamanhoSombra") %> #000;
    -moz-box-shadow: 0px 0px <%=Util.Configuracao.AppSettings("VETamanhoSombra")%> #000;
    -webkit-box-shadow: 0px 0px <%=Util.Configuracao.AppSettings("VETamanhoSombra")%> #000;
    border-radius: <%=Util.Configuracao.AppSettings("VETamanhoCurvaBorda")%>;
    -moz-border-radius: <%=Util.Configuracao.AppSettings("VETamanhoCurvaBorda")%>;
    -webkit-border-radius: <%=Util.Configuracao.AppSettings("VETamanhoCurvaBorda")%>;
}

.formError .formErrorArrow div{
	box-shadow: 1px  <%=Util.Configuracao.AppSettings("VETamanhoSombra")%> <%=Util.Configuracao.AppSettings("VETamanhoSombra")%> #666;
	-moz-box-shadow: 1px  <%=Util.Configuracao.AppSettings("VETamanhoSombra")%> <%=Util.Configuracao.AppSettings("VETamanhoSombra")%> #666;
	-webkit-box-shadow: 1px  <%=Util.Configuracao.AppSettings("VETamanhoSombra")%> <%=Util.Configuracao.AppSettings("VETamanhoSombra")%> #666;
	background: <%=Util.Configuracao.AppSettings("VECorFundo") %>;
}