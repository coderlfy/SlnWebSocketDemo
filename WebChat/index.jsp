<%@ page language="java" contentType="text/html; charset=UTF-8" pageEncoding="UTF-8"%>
<%
	String message_to = request.getParameter( "message_to" );
    String message_me = request.getParameter( "message_me" );
    request.setAttribute( "message_to" , message_to );
    request.setAttribute( "message_me" , message_me );
%>
<html>
<head>
<title></title>
<style>
</style>
<script type="text/javascript" src="scripts/swfobject.js"></script>
<script type="text/javascript" src="scripts/jquery.js"></script>
<script type="text/javascript" src="scripts/web_socket.js"></script>
<script type="text/javascript" src="scripts/jquery.WebSocket.js"></script>

<script>
var ws;
var WEB_SOCKET_SWF_LOCATION = 'media/WebSocketMain.swf';
var WEB_SOCKET_DEBUG = true;
	$(function() {
		$("#sendBtn").on("click",send);
		 connection();
	});
	
	var message_to = "${message_to}";
	var message_me = "${message_me}";
	//message_to="1111";
	//message_me="222";
	function connection(){
		   ws = $.websocket({  
		        domain:"172.35.0.128", 
		        protocol:"demo01/MyHtml.html?message_to="+message_to+"&message_me="+message_me, 
		        port:"8080",
		        onOpen:function(event){  
		        	showMessage("已成功登录");  
		        },  
		        onError:function(event){
		         	alert("error:"+ event)
		        },  
		        onMessage:function(result){  
		            if(result!="1")
		        	  receiveMessage(result);
		        },
		        onClose:function(event){
		        	ws = null;
			    }
		    });  
	}
 
	function send(){
		if(!ws){
			alert("已经断开聊天室");
			return;
		}
		 var msg=$.trim($("#messageInput").val());
		 if(msg==""){return;}
		 ws.send(msg);  
		 $("#messageInput").val("").focus();;
	}
	
	function closeCon(){
	   ws.close();
	}
	
	
	function receiveMessage(result){
		showMessage(result);
	}
 
	function showMessage(msg){
		$("#public").append("<div>"+msg+"</div>");
	}
	 
	 
</script>
</head>
<body>
 <div id="public" style="height:500px;border:1px solid;"></div>
 
 
		<div class="input">
					<input type="text" id="messageInput" onKeyDown="if (event.keyCode==13)$('#sendBtn').click()" maxlength="100" size="40" tabindex="1" /> 
					<input type="button"  id="sendBtn" value="提交" >
					<br/>
					<input type="button"  id="closeBtn" onclick="closeCon();" value="断开" > 
		</div>
	 
</body>
</html>