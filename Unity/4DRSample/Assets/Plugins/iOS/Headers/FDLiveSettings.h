/*
 * Copyright (c) 2019-2020 4dreplay Co., Ltd.
 * All rights reserved.
 */

#import <Foundation/Foundation.h>

#define FDSettings ([FDLiveSettings sharedInstance])

NS_ASSUME_NONNULL_BEGIN

@interface FDLiveSettings : NSObject

@property (nonatomic, strong, readonly) NSMutableDictionary *config;
@property (nonatomic, getter=getTimeout, readonly) NSUInteger timeout;
@property (nonatomic, getter=getBaseUrl, readonly) NSString* baseUrl;
@property (nonatomic, getter=getServerIP, readonly) NSString* serverIP;
@property (nonatomic, getter=getServerPort, readonly) NSInteger serverPort;
@property (nonatomic, getter=getVersion, readonly) NSString *version;
@property (nonatomic, strong, readonly) NSMutableDictionary *dictionaryForQueue;

+ (FDLiveSettings *)sharedInstance;

/* Give global configuation for a GlobalSetting. */
- (int)set:(NSString *)key string:(NSString *)value;
- (int)set:(NSString *)key integer:(NSInteger)value;

@end

NS_ASSUME_NONNULL_END
