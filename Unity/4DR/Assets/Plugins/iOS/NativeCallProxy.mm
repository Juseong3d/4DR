#import <Foundation/Foundation.h>
#import "NativeCallProxy.h"


@implementation FrameworkLibAPI

id<NativeCallsProtocol> api = NULL;
+(void) registerAPIforNativeCalls:(id<NativeCallsProtocol>) aApi
{
    api = aApi;
}

@end


extern "C" {
    void amazerAppMessage(const char* type, const char* message)
    {
        return [api amazerApp:[NSString stringWithUTF8String:type] message:[NSString stringWithUTF8String:message]];
    }
}

