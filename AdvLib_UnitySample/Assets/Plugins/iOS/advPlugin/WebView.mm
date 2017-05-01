/*
 * Copyright (C) 2011 Keijiro Takahashi
 * Copyright (C) 2012 GREE, Inc.
 *
 * This software is provided 'as-is', without any express or implied
 * warranty.  In no event will the authors be held liable for any damages
 * arising from the use of this software.
 *
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 *
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would be
 *    appreciated but is not required.
 * 2. Altered source versions must be plainly marked as such, and must not be
 *    misrepresented as being the original software.
 * 3. This notice may not be removed or altered from any source distribution.
 */

#import <UIKit/UIKit.h>

extern UIViewController *UnityGetGLViewController();

@interface WebViewPlugin : UIViewController<UIWebViewDelegate>
{
    UIWebView *webView;
    NSString *gameObjectName;
    UINavigationBar *navigationBar;
    UIView *view;
}

@end

@implementation WebViewPlugin

- (id)initWithGameObjectName:(const char *)gameObjectName_
{
    self = [super init];
    view = UnityGetGLViewController().view;
    webView = [[UIWebView alloc] initWithFrame:view.frame];
    webView.delegate = self;
    webView.frame = CGRectMake(0, 50,view.frame.size.width ,view.frame.size.height);
    [view addSubview:webView];
    gameObjectName = [NSString stringWithUTF8String:gameObjectName_];
    
    webView.hidden = YES;
    navigationBar.hidden = YES;
    
    return self;
}

- (void)dealloc
{
    webView.delegate = nil;
    
    [webView removeFromSuperview];
    [super dealloc];
}

- (void)close
{
    CGRect webViewFrame =  CGRectMake(-[UIScreen mainScreen].bounds.size.width, webView.frame.origin.y, webView.frame.size.width, webView.frame.size.height);
    CGRect navigationBarFrame =  CGRectMake(-[UIScreen mainScreen].bounds.size.width, navigationBar.frame.origin.y, navigationBar.frame.size.width, navigationBar.frame.size.height);
    
    [UIView animateWithDuration:1 delay:1.0 options:UIViewAnimationOptionCurveEaseIn animations:^{
        webView.frame = webViewFrame;
        navigationBar.frame = navigationBarFrame;
    }  completion:^(BOOL finished){
        webView.hidden = YES;
        navigationBar.hidden = YES;
    }];
}

- (BOOL)webView:(UIWebView *)webViewReturn shouldStartLoadWithRequest:(NSURLRequest *)request navigationType:(UIWebViewNavigationType)navigationType
{
    NSURL* theURL = [request mainDocumentURL];
    NSString* absoluteString = [theURL absoluteString];
    NSString* scene = [[absoluteString componentsSeparatedByString:@"#"] lastObject];
    if ([scene caseInsensitiveCompare:@"scene1"]==0 || [scene caseInsensitiveCompare:@"scene2"]==0)
    {
        const char *name = [scene UTF8String];
        
        UnitySendMessage("MyClass", "getWebViewMsg", name);
        
        [webViewReturn removeFromSuperview];
        webViewReturn = nil;
    }
    
    return YES;
}

- (void)webViewDidStartLoad: (UIWebView*)webView
{
    
    NSString* url = [[[webView request] URL] absoluteString];
    UnitySendMessage( [gameObjectName UTF8String], "onLoadStart", [url UTF8String] );
}

- (void)webViewDidFinishLoad: (UIWebView*)webView
{
    NSString* url = [webView stringByEvaluatingJavaScriptFromString:@"document.URL"];
    UnitySendMessage( [gameObjectName UTF8String], "onLoadFinish", [url UTF8String] );
}
- (void)webViewDidFailLoadWithError: (NSError*)error
{
    [UIApplication sharedApplication].networkActivityIndicatorVisible = NO;
    
    NSInteger err_code = [error code];
    if( err_code == NSURLErrorCancelled )
    {
        return;
    }
}

- (void)setMargins:(int)width height:(int)height x:(int)x y:(int)y
{
    CGRect frame = view.frame;
    frame.size.width = width;
    frame.size.height = height;
    frame.origin.x = x;
    frame.origin.y = y;
    webView.frame = frame;
}

- (void)setNavBar: (const char*) title width:(int)width height:(int)height x:(int)x y:(int)y btnImg:(const char*)btnImg btnwidth:(int)btnwidth btnheight:(int)btnheight btnx:(int)btnx btny:(int)btny bgImg:(const char*)bgImg
{
    //initiate the navigate bar
    UINavigationController *addViewControl = [[UINavigationController alloc] init];
    addViewControl.navigationBarHidden = NO;
    addViewControl.view.frame = CGRectMake(0, 0, view.frame.size.width, 50);
    UIImage *image = [UIImage imageWithData:[NSData dataWithContentsOfURL:[NSURL URLWithString:[NSString stringWithFormat:@"%s", bgImg]]]];
    navigationBar = [[UINavigationBar alloc] initWithFrame:CGRectMake(x,y,width, height)];
    [navigationBar setBackgroundImage:image forBarMetrics:UIBarMetricsDefault];
    
    //initiate the left back button
    UIView *customView = [[UIView alloc] initWithFrame:CGRectMake(btnx,btny,btnwidth, btnheight)];
    UIButton *button = [[UIButton alloc] initWithFrame:customView.frame];
    [button addTarget:self action:@selector(close) forControlEvents:UIControlEventTouchUpInside];
    UIImage *image2 = [UIImage imageWithData:[NSData dataWithContentsOfURL:[NSURL URLWithString:[NSString stringWithFormat:@"%s", btnImg]]]];
    [button setImage:image2 forState:UIControlStateNormal];
    [customView addSubview:button];
    UIBarButtonItem *back = [[UIBarButtonItem alloc] initWithCustomView:customView];
    
    //initiate the title
    UINavigationItem *mytitle = [[UINavigationItem alloc] initWithTitle:[NSString stringWithUTF8String:title]];
    mytitle.leftBarButtonItem = back;
    
    navigationBar.items = @[mytitle];
    
    [view addSubview:navigationBar];
}

- (void)setVisibility:(BOOL)visibility
{
    webView.hidden = visibility ? NO : YES;
    navigationBar.hidden = visibility ? NO : YES;
    
    [self startAnimation];
}

-(void) startAnimation
{
    CGRect webViewFrame = webView.frame;
    CGRect navigationBarFrame = navigationBar.frame;
    
    webView.frame =  CGRectMake([UIScreen mainScreen].bounds.size.width, webViewFrame.origin.y, webViewFrame.size.width, webViewFrame.size.height);
    navigationBar.frame =  CGRectMake([UIScreen mainScreen].bounds.size.width, navigationBarFrame.origin.y, navigationBarFrame.size.width, navigationBarFrame.size.height);
    
    [UIView animateWithDuration:1 delay:1.0 options:UIViewAnimationOptionCurveEaseIn animations:^{
        webView.frame = webViewFrame;
        navigationBar.frame = navigationBarFrame;
    }  completion:^(BOOL finished){
        NSLog(@"Done!");
    }];
}

- (void)loadURL:(const char *)url
{
    NSString *urlStr = [NSString stringWithUTF8String:url];
    NSURL *nsurl = [NSURL URLWithString:urlStr];
    NSURLRequest *request = [NSURLRequest requestWithURL:nsurl];
    
    [webView loadRequest:request];
    [webView reload];
}

- (void)evaluateJS:(const char *)js
{
    NSString *jsStr = [NSString stringWithUTF8String:js];
    [webView stringByEvaluatingJavaScriptFromString:jsStr];
}



@end

extern "C" {
    void *_WebViewPlugin_Init(const char *gameObjectName);
    void _WebViewPlugin_Destroy(void *instance);
    void _WebViewPlugin_SetMargins(void *instance, int width, int height, int x, int y);
    void _WebViewPlugin_SetNavigateBar(void *instance, const char *title, int width, int height, int x, int y, const char * btnImg, int btnWidth, int btnHight, int btnX, int btnY, const char * bgImg);
    void _WebViewPlugin_SetVisibility(void *instance, BOOL visibility);
    void _WebViewPlugin_LoadURL(void *instance, const char *url);
    void _WebViewPlugin_EvaluateJS(void *instance, const char *url);
}

void *_WebViewPlugin_Init(const char *gameObjectName)
{
    id instance = [[WebViewPlugin alloc] initWithGameObjectName:gameObjectName];
    return (__bridge void *)instance;
}

void _WebViewPlugin_Destroy(void *instance)
{
    
}

void _WebViewPlugin_SetMargins(void *instance, int width, int height, int x, int y)
{
    WebViewPlugin *webViewPlugin = (WebViewPlugin *)instance;
    [webViewPlugin setMargins:width height:height x:x y:y];
}

void _WebViewPlugin_SetNavigateBar(void *instance, const char *title, int width, int height, int x, int y, const char * btnImg, int btnWidth, int btnHight, int btnX, int btnY, const char * bgImg)
{
    WebViewPlugin *webViewPlugin = (WebViewPlugin *)instance;
    [webViewPlugin setNavBar:title width:width height:height x:x y:y btnImg:btnImg btnwidth:btnWidth btnheight:btnHight btnx:btnX btny:btnY bgImg:bgImg];
}

void _WebViewPlugin_SetVisibility(void *instance, BOOL visibility)
{
    WebViewPlugin *webViewPlugin = (WebViewPlugin *)instance;
    [webViewPlugin setVisibility:visibility];
}

void _WebViewPlugin_LoadURL(void *instance, const char *url)
{
    NSLog(@"url:%s",url);
    WebViewPlugin *webViewPlugin = (WebViewPlugin *)instance;
    [webViewPlugin loadURL:url];
}

void _WebViewPlugin_EvaluateJS(void *instance, const char *js)
{
    WebViewPlugin *webViewPlugin = (WebViewPlugin *)instance;
    [webViewPlugin evaluateJS:js];
}
