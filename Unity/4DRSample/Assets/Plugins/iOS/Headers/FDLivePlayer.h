/*
* Copyright (c) 2019-2020 4dreplay Co., Ltd.
* All rights reserved.
*/

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import "FDLiveDefines.h"

NS_ASSUME_NONNULL_BEGIN

/**
* Represents a 4dreplay's 4D live play.
*
* This class provides a streaming playback by 4dreplay input/output definition paper.
*/

@class FDLivePlayer;

typedef void (^ResponseHandler)(NSData *data, NSURLResponse *response, NSError *error);

@protocol FDPlayerDelegate <NSObject>
@optional

- (void)getCurrentPlayInfo:(FDLivePlayer *)player channel:(int)channel
                     frame:(int)frame frameCycle:(int)frameCycle time:(int)time utc:(NSString *)utc;
- (void)getVideoStreamInfo:(FDLivePlayer *)player width:(int)width height:(int)height duration:(int)duration;
- (void)getStart:(FDLivePlayer *)player code:(int)code;
- (void)getStop:(FDLivePlayer *)player code:(int)code;
- (void)getPlay:(FDLivePlayer *)player;
- (void)getPause:(FDLivePlayer *)player;
- (void)getPlayDone:(FDLivePlayer *)player;
- (void)getSlowReceiveFrame:(FDLivePlayer *)player slow:(int)isSlow;
- (void)getError:(FDLivePlayer *)player code:(int)code message:(NSString *)message;

/* Dprecated delegate */
- (void)getCurrentPlayInfo:(int)channel frame:(int)frame
                frameCycle:(int)frameCycle time:(int)time utc:(NSString *)utc __deprecated;
- (void)getVideoStreamInfo:(int)width height:(int)height duration:(int)duration __deprecated;
- (void)getStart:(int)code __deprecated;
- (void)getStop:(int)code __deprecated;
- (void)getPlay __deprecated;
- (void)getPause __deprecated;
- (void)getPlayDone __deprecated;
- (void)getSlowReceiveFrame:(int)isSlow __deprecated;
- (void)getError:(int)code message:(NSString *)message __deprecated;

@end

@interface FDLivePlayer : NSObject
{
@protected
    NSString *_sessionID;
}

@property (nonatomic, weak) id<FDPlayerDelegate>delegate;
@property (nonatomic, assign) int streamOpenStartTS;
@property (nonatomic, assign) BOOL isLoop; /* Set to play the VOD content again, even if it completes. */
@property (nonatomic, strong, readonly) NSString *sessionID; /* Unique key for communicating with the FDLive server. */
@property (nonatomic, strong, readonly) UIView *playerView;
@property (nonatomic, assign, readonly) FD_PLAYER_STATE state;

@property (atomic, assign) int curMsec;
@property (nonatomic, copy) ResponseHandler responseHandler;

/* Constructor */
- (instancetype)initWithDelegate:(id<FDPlayerDelegate>)delegate;

/* Setting server ip and port number */
- (int)RESTFulOpen:(NSString *)serverIP port:(NSInteger)port;
- (int)streamOpen:(NSString *)url isTCP:(BOOL)isTCP isHWAccel:(BOOL)isHWAccel;
- (int)streamClose;

- (int)play;
- (int)playToNow;
- (int)pause;
- (int)seek:(NSInteger)msec;
- (int)speed:(CGFloat)speed;

/* Change the channel according to input value. */
- (int)setChangeChannel:(NSString *)actiontype direction:(NSString *)direction moveFrame:(NSInteger)moveFrame;
/*
 * Used for video channel and time control when pause state.
 * Support pause/play when live mode.
 */
- (int)setChangeFrameCh:(NSString *)playtype direction:(NSString *)direction;
/* Change to the defined video channel position. */
- (int)setPositionChange:(NSString *)position;
@end

NS_ASSUME_NONNULL_END
