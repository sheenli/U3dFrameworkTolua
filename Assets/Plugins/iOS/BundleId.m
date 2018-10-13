#import "BundleId.h"
#include <sys/socket.h>
#include <netdb.h>
#include <arpa/inet.h>
#include <err.h>

#define MakeStringCopy( _x_ ) ( _x_ != NULL && [_x_ isKindOfClass:[NSString class]] ) ? strdup( [_x_ UTF8String] ) : NULL

const char* getIPv6(const char *mHost,const char *mPort)
{
	if( nil == mHost )
		return NULL;
	const char *newChar = "No";
	const char *cause = NULL;
	struct addrinfo* res0;
	struct addrinfo hints;
	struct addrinfo* res;
	int n, s;
	
	memset(&hints, 0, sizeof(hints));
	
	hints.ai_flags = AI_DEFAULT;
	hints.ai_family = PF_UNSPEC;
	hints.ai_socktype = SOCK_STREAM;
	
	if((n=getaddrinfo(mHost, "http", &hints, &res0))!=0)
	{
		printf("getaddrinfo error: %s\n",gai_strerror(n));
		return NULL;
	}
	
	struct sockaddr_in6* addr6;
	struct sockaddr_in* addr;
	NSString * NewStr = NULL;
	char ipbuf[32];
	s = -1;
	for(res = res0; res; res = res->ai_next)
	{
		if (res->ai_family == AF_INET6)
		{
			addr6 =( struct sockaddr_in6*)res->ai_addr;
			newChar = inet_ntop(AF_INET6, &addr6->sin6_addr, ipbuf, sizeof(ipbuf));
			NSString * TempA = [[NSString alloc] initWithCString:(const char*)newChar 
encoding:NSASCIIStringEncoding];
			NSString * TempB = [NSString stringWithUTF8String:"&&ipv6"];
			
			NewStr = [TempA stringByAppendingString: TempB];
			printf("%s\n", newChar);
		}
		else
		{
			addr =( struct sockaddr_in*)res->ai_addr;
			newChar = inet_ntop(AF_INET, &addr->sin_addr, ipbuf, sizeof(ipbuf));
			NSString * TempA = [[NSString alloc] initWithCString:(const char*)newChar 
encoding:NSASCIIStringEncoding];
			NSString * TempB = [NSString stringWithUTF8String:"&&ipv4"];
			
			NewStr = [TempA stringByAppendingString: TempB];			
			printf("%s\n", newChar);
		}
		break;
	}
	
	
	freeaddrinfo(res0);
	
	printf("getaddrinfo OK");
	
	NSString * mIPaddr = NewStr;
	return MakeStringCopy(mIPaddr);
}
