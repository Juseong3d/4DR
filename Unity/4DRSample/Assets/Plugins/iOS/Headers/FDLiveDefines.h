/*
* Copyright (c) 2019-2020 4dreplay Co., Ltd.
* All rights reserved.
*/

#ifndef FDLiveDefines_h
#define FDLiveDefines_h

#define    FDLIVE_VERSION_STRING @"1.1.0.2020072700"
#define    FDLIVE_VERSION_NUMBER @"10100"

/* Define FD player state */
typedef enum : NSUInteger {
    FD_STATE_NONE,
    FD_STATE_OPENING,
    FD_STATE_OPEN,
    FD_STATE_PLAY,
    FD_STATE_PAUSE,
    FD_STATE_EOF,
    FD_STATE_CLOSING,
    FD_STATE_CLOSE,
    FD_STATE_ERROR
} FD_PLAYER_STATE;

/* Define camera position */
typedef enum : NSUInteger {
    FD_POS_P,
    FD_POS_C,
    FD_POS_1,
    FD_POS_2,
    FD_POS_3,
    FD_POS_1_2,
    FD_POS_2_3,
} FD_POS_TYPE;

typedef enum : NSUInteger{
    FD_SUCESS = 0,  /* Function sucess */
    /* Initialize */
    FD_ERR_INI_1000 = 1000, /* An unknown error has occurred during initialization. */
    FD_ERR_INI_1010 = 1010, /* Invalid input value. */
    FD_ERR_INI_1100 = 1100, /* The server address for the video control not valid. */
    
    /* Engine */
    FD_ERR_ENG_2001 = 2001,
    FD_ERR_ENG_2002 = 2002,
    FD_ERR_ENG_2003 = 2003,
    FD_ERR_ENG_2004 = 2004,
    FD_ERR_ENG_2005 = 2005,
    FD_ERR_ENG_2006 = 2006,
    FD_ERR_ENG_2007 = 2007,
    FD_ERR_ENG_2008 = 2008,
    FD_ERR_ENG_2009 = 2009,
    FD_ERR_ENG_2200 = 2200,
    FD_ERR_ENG_2203 = 2203,
    FD_ERR_ENG_2204 = 2204,
    FD_ERR_ENG_2205 = 2205,

    /* Working function */
    FD_ERR_FTN_3000 = 3000, /* An unknown error occurred while executing the function. */
    FD_ERR_FTN_3010 = 3010, /* Invalid input value. */
    /* Stream open */
    FD_ERR_FTN_3100 = 3100, /* Fail to stream open. */
    FD_ERR_FTN_3110 = 3110, /* The streaming start time value is invalid. */
    FD_ERR_FTN_3120 = 3120, /* Streaming already opened. */
    FD_ERR_FTN_3130 = 3130, /* Session id creation failed. */
    FD_ERR_FTN_3190 = 3190, /* Streaming is not open. */
    /* Stream close */
    FD_ERR_FTN_3200 = 3200, /* Player already closed. */
    FD_ERR_FTN_3210 = 3210, /* It is a problem terminating the stream. */
    /* Pause */
    FD_ERR_FTN_3400 = 3400, /* This channel do not stop channel. */
    FD_ERR_FTN_3410 = 3410, /* There is currently no frame in live mode. */
    /* Seek */
    FD_ERR_FTN_3500 = 3500, /* There is no internal condition to perform this function. */
    FD_ERR_FTN_3510 = 3510, /* Input value is limit over. */
    FD_ERR_FTN_3520 = 3520, /* Invalid input value, lower than zero. */
    
    /* Internal newtwork */
    FD_ERR_NET_5000 = 5000, /* It is internal newtwork error. */
    FD_ERR_NET_5001 = 5001  /* It is printed when on alternate view. Sreen control is not available. */
} FD_ERROR_CODE;

#endif /* FDLiveDefines_h */
