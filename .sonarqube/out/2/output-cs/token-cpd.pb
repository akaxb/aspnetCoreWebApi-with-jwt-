ê
CC:\Users\akaxb\source\repos\MicroServices\Resilience\IHttpClient.cs
	namespace 	

Resilience
 
{ 
public 

	interface 
IHttpClient  
{		 
Task

 
<

 
HttpResponseMessage

  
>

  !
	PostAsync

" +
<

+ ,
T

, -
>

- .
(

. /
string

/ 5
uri

6 9
,

9 :
T

: ;
item

< @
,

@ A
string

A G
authorizationToken

H Z
,

Z [
string

[ a
	requestId

b k
=

k l
null

l p
,

p q
string

q w 
authorizationMethod	

x ã
=


ã å
$str


å î
)


î ï
;


ï ñ
Task 
< 
HttpResponseMessage  
>  !
	PostAsync" +
(+ ,
string, 2
uri3 6
,6 7

Dictionary8 B
<B C
stringC I
,I J
stringJ P
>P Q
formR V
,V W
stringX ^
authorizationToken_ q
=q r
nullr v
,v w
stringx ~
	requestId	 à
=
â ä
null
ã è
,
è ê
string
ë ó!
authorizationMethod
ò ´
=
¨ ≠
$str
Æ ∂
)
∂ ∑
;
∑ ∏
} 
} Õj
LC:\Users\akaxb\source\repos\MicroServices\Resilience\ResilienceHttpClient.cs
	namespace 	

Resilience
 
{ 
public 

class  
ResilienceHttpClient %
:& '
IHttpClient( 3
{ 
private 
readonly 

HttpClient #
_httpClient$ /
;/ 0
private 
readonly 
Func 
< 
string $
,$ %
IEnumerable& 1
<1 2
Policy2 8
>8 9
>9 :
_policyCreator; I
;I J
private 
readonly  
ConcurrentDictionary -
<- .
string. 4
,4 5

PolicyWrap6 @
>@ A
_policyWrappersB Q
;Q R
private 
ILogger 
<  
ResilienceHttpClient ,
>, -
_logger. 5
;5 6
private  
IHttpContextAccessor $ 
_httpContextAccessor% 9
;9 :
public  
ResilienceHttpClient #
(# $
Func$ (
<( )
string) /
,/ 0
IEnumerable1 <
<< =
Policy= C
>C D
>D E
policyCreatorF S
,S T
ILoggerU \
<\ ] 
ResilienceHttpClient] q
>q r
loggers y
,y z!
IHttpContextAccessor	{ è!
httpContextAccessor
ê £
)
£ §
{ 	
_httpClient 
= 
new 

HttpClient (
(( )
)) *
;* +
_policyCreator   
=   
policyCreator   *
;  * +
_policyWrappers!! 
=!! 
new!! ! 
ConcurrentDictionary!!" 6
<!!6 7
string!!7 =
,!!= >

PolicyWrap!!? I
>!!I J
(!!J K
)!!K L
;!!L M
_logger"" 
="" 
logger"" 
;""  
_httpContextAccessor##  
=##! "
httpContextAccessor### 6
;##6 7
}$$ 	
public&& 
async&& 
Task&& 
<&& 
HttpResponseMessage&& -
>&&- .
	PostAsync&&/ 8
<&&8 9
T&&9 :
>&&: ;
(&&; <
string&&< B
uri&&C F
,&&F G
T&&H I
item&&J N
,&&N O
string&&P V
authorizationToken&&W i
,&&i j
string&&k q
	requestId&&r {
=&&| }
null	&&~ Ç
,
&&Ç É
string
&&Ñ ä!
authorizationMethod
&&ã û
=
&&ü †
$str
&&° ©
)
&&© ™
{'' 	
Func(( 
<(( 
HttpRequestMessage(( #
>((# $
func((% )
=((* +
(((, -
)((- .
=>((/ 1$
CreateHttpRequestMessage((2 J
(((J K

HttpMethod((K U
.((U V
Post((V Z
,((Z [
uri((\ _
,((_ `
item((a e
)((e f
;((f g
return)) 
await)) 
DoPostPutAsync)) '
())' (

HttpMethod))( 2
.))2 3
Post))3 7
,))7 8
uri))9 <
,))< =
func))> B
,))B C
authorizationToken))D V
,))V W
	requestId))X a
,))a b
authorizationMethod))c v
)))v w
;))w x
}** 	
private,, 
Task,, 
<,, 
HttpResponseMessage,, (
>,,( )
DoPostPutAsync,,* 8
(,,8 9

HttpMethod,,9 C
method,,D J
,,,J K
string,,L R
uri,,S V
,,,V W
Func,,X \
<,,\ ]
HttpRequestMessage,,] o
>,,o p
func,,q u
,,,u v
string,,w }
authorizationToken	,,~ ê
=
,,ë í
null
,,ì ó
,
,,ó ò
string
,,ô ü
	requestId
,,† ©
=
,,™ ´
null
,,¨ ∞
,
,,∞ ±
string
,,≤ ∏!
authorizationMethod
,,π Ã
=
,,Õ Œ
$str
,,œ ◊
)
,,◊ ÿ
{-- 	
if.. 
(.. 
method.. 
!=.. 

HttpMethod.. $
...$ %
Post..% )
&&..* ,
method..- 3
!=..4 6

HttpMethod..7 A
...A B
Put..B E
)..E F
{// 
throw00 
new00 
ArgumentException00 +
(00+ ,
$str00, O
,00O P
nameof00Q W
(00W X
method00X ^
)00^ _
)00_ `
;00` a
}11 
var44 
origin44 
=44 
GetOriginFromUri44 )
(44) *
uri44* -
)44- .
;44. /
return55 
HttpInvoker55 
(55 
origin55 %
,55% &
async55' ,
(55- .
)55. /
=>550 2
{66 
var77 
requestMessage77 "
=77# $
func77% )
(77) *
)77* +
;77+ ,"
SetAuthorizationHeader88 &
(88& '
requestMessage88' 5
)885 6
;886 7
if:: 
(:: 
authorizationToken:: &
!=::' )
null::* .
)::. /
{;; 
requestMessage<< "
.<<" #
Headers<<# *
.<<* +
Authorization<<+ 8
=<<9 :
new<<; >%
AuthenticationHeaderValue<<? X
(<<X Y
authorizationMethod<<Y l
,<<l m
authorizationToken	<<n Ä
)
<<Ä Å
;
<<Å Ç
}== 
if>> 
(>> 
	requestId>> 
!=>>  
null>>! %
)>>% &
{?? 
requestMessage@@ "
.@@" #
Headers@@# *
.@@* +
Add@@+ .
(@@. /
$str@@/ <
,@@< =
	requestId@@> G
)@@G H
;@@H I
}AA 
varBB 
responseBB 
=BB 
awaitBB $
_httpClientBB% 0
.BB0 1
	SendAsyncBB1 :
(BB: ;
requestMessageBB; I
)BBI J
;BBJ K
ifEE 
(EE 
responseEE 
.EE 

StatusCodeEE '
==EE( *
HttpStatusCodeEE+ 9
.EE9 :
InternalServerErrorEE: M
)EEM N
{FF 
throwGG 
newGG  
HttpRequestExceptionGG 2
(GG2 3
)GG3 4
;GG4 5
}HH 
returnII 
responseII 
;II  
}JJ 
)JJ 
;JJ 
}KK 	
privateNN 
HttpRequestMessageNN "$
CreateHttpRequestMessageNN# ;
<NN; <
TNN< =
>NN= >
(NN> ?

HttpMethodNN? I
methodNNJ P
,NNP Q
stringNNR X
uriNNY \
,NN\ ]
TNN^ _
itemNN` d
)NNd e
{OO 	
returnPP 
newPP 
HttpRequestMessagePP )
(PP) *
methodPP* 0
,PP0 1
uriPP2 5
)PP5 6
{QQ 
ContentRR 
=RR 
newRR 
StringContentRR +
(RR+ ,
JsonConvertRR, 7
.RR7 8
SerializeObjectRR8 G
(RRG H
itemRRH L
)RRL M
,RRM N
EncodingRRO W
.RRW X
UTF8RRX \
,RR\ ]
$strRR^ p
)RRp q
}SS 
;SS 
}TT 	
privateUU 
HttpRequestMessageUU "$
CreateHttpRequestMessageUU# ;
(UU; <

HttpMethodUU< F
methodUUG M
,UUM N
stringUUO U
uriUUV Y
,UUY Z

DictionaryUU[ e
<UUe f
stringUUf l
,UUl m
stringUUn t
>UUt u
formUUv z
)UUz {
{VV 	
returnWW 
newWW 
HttpRequestMessageWW )
(WW) *
methodWW* 0
,WW0 1
uriWW2 5
)WW5 6
{WW7 8
ContentWW9 @
=WWA B
newWWC F!
FormUrlEncodedContentWWG \
(WW\ ]
formWW] a
)WWa b
}WWc d
;WWd e
}XX 	
private[[ 
async[[ 
Task[[ 
<[[ 
T[[ 
>[[ 
HttpInvoker[[ )
<[[) *
T[[* +
>[[+ ,
([[, -
string[[- 3
origin[[4 :
,[[: ;
Func[[< @
<[[@ A
Task[[A E
<[[E F
T[[F G
>[[G H
>[[H I
action[[J P
)[[P Q
{\\ 	
var]] 
normalizedOrigin]]  
=]]! "
NormalizeOrigin]]# 2
(]]2 3
origin]]3 9
)]]9 :
;]]: ;
if^^ 
(^^ 
!^^ 
_policyWrappers^^  
.^^  !
TryGetValue^^! ,
(^^, -
normalizedOrigin^^- =
,^^= >
out^^? B

PolicyWrap^^C M

policyWrap^^N X
)^^X Y
)^^Y Z
{__ 

policyWrap`` 
=`` 
Policy`` #
.``# $
	WrapAsync``$ -
(``- .
_policyCreator``. <
(``< =
normalizedOrigin``= M
)``M N
.``N O
ToArray``O V
(``V W
)``W X
)``X Y
;``Y Z
_policyWrappersaa 
.aa  
TryAddaa  &
(aa& '
normalizedOriginaa' 7
,aa7 8

policyWrapaa9 C
)aaC D
;aaD E
}bb 
returnff 
awaitff 

policyWrapff #
.ff# $
ExecuteAsyncff$ 0
(ff0 1
actionff1 7
,ff7 8
newff9 <
Contextff= D
(ffD E
normalizedOriginffE U
)ffU V
)ffV W
;ffW X
}gg 	
privateii 
staticii 
stringii 
NormalizeOriginii -
(ii- .
stringii. 4
originii5 ;
)ii; <
{jj 	
returnkk 
originkk 
?kk 
.kk 
Trimkk 
(kk  
)kk  !
?kk! "
.kk" #
ToLowerkk# *
(kk* +
)kk+ ,
;kk, -
}ll 	
privatenn 
staticnn 
stringnn 
GetOriginFromUrinn .
(nn. /
stringnn/ 5
urinn6 9
)nn9 :
{oo 	
varpp 
urlpp 
=pp 
newpp 
Uripp 
(pp 
uripp !
)pp! "
;pp" #
varqq 
originqq 
=qq 
$"qq 
{qq 
urlqq 
.qq  
Schemeqq  &
}qq& '
://qq' *
{qq* +
urlqq+ .
.qq. /
DnsSafeHostqq/ :
}qq: ;
:qq; <
{qq< =
urlqq= @
.qq@ A
PortqqA E
}qqE F
"qqF G
;qqG H
returnrr 
originrr 
;rr 
}ss 	
privatett 
voidtt "
SetAuthorizationHeadertt +
(tt+ ,
HttpRequestMessagett, >
requestMessagett? M
)ttM N
{uu 	
varvv 
authorizationHeadervv #
=vv$ % 
_httpContextAccessorvv& :
.vv: ;
HttpContextvv; F
.vvF G
RequestvvG N
.vvN O
HeadersvvO V
[vvV W
$strvvW f
]vvf g
;vvg h
ifww 
(ww 
!ww 
stringww 
.ww 
IsNullOrEmptyww %
(ww% &
authorizationHeaderww& 9
)ww9 :
)ww: ;
{xx 
requestMessageyy 
.yy 
Headersyy &
.yy& '
Addyy' *
(yy* +
$stryy+ :
,yy: ;
newyy< ?
Listyy@ D
<yyD E
stringyyE K
>yyK L
(yyL M
)yyM N
{yyO P
authorizationHeaderyyQ d
}yye f
)yyf g
;yyg h
}zz 
}{{ 	
public}} 
async}} 
Task}} 
<}} 
HttpResponseMessage}} -
>}}- .
	PostAsync}}/ 8
(}}8 9
string}}9 ?
uri}}@ C
,}}C D

Dictionary}}E O
<}}O P
string}}P V
,}}V W
string}}X ^
>}}^ _
form}}` d
,}}d e
string}}f l
authorizationToken}}m 
=
}}Ä Å
null
}}Ç Ü
,
}}Ü á
string
}}à é
	requestId
}}è ò
=
}}ô ö
null
}}õ ü
,
}}ü †
string
}}° ß!
authorizationMethod
}}® ª
=
}}º Ω
$str
}}æ ∆
)
}}∆ «
{~~ 	
Func 
< 
HttpRequestMessage #
># $
func% )
=* +
(, -
)- .
=>/ 1$
CreateHttpRequestMessage2 J
(J K

HttpMethodK U
.U V
PostV Z
,Z [
uri\ _
,_ `
forma e
)e f
;f g
return
ÄÄ 
await
ÄÄ 
DoPostPutAsync
ÄÄ '
(
ÄÄ' (

HttpMethod
ÄÄ( 2
.
ÄÄ2 3
Post
ÄÄ3 7
,
ÄÄ7 8
uri
ÄÄ9 <
,
ÄÄ< =
func
ÄÄ> B
,
ÄÄB C 
authorizationToken
ÄÄD V
,
ÄÄV W
	requestId
ÄÄX a
,
ÄÄa b!
authorizationMethod
ÄÄc v
)
ÄÄv w
;
ÄÄw x
}
ÅÅ 	
}
ÇÇ 
}ÉÉ 