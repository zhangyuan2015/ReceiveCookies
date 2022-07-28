using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReceiveCookies.Core.Model;
using ReceiveCookies.Core.Utils;

namespace ReceiveCookies.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReceiveController : ControllerBase
    {
        private readonly ILogger<ReceiveController> _logger;
        private IConfiguration _configuration;

        public ReceiveController(ILogger<ReceiveController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        public List<CookieModel> Get()
        {
            return new List<CookieModel> {
                new CookieModel {
                    domain = "示例:github.com",
                    expirationDate = 1686661592.931744,
                    hostOnly = true,
                    httpOnly = true,
                    name = "_device_id",
                    path = "/",
                    sameSite = "lax",
                    secure = true,
                    session = false,
                    storeId = "0",
                    value= "********"
                }
            };
        }

        [HttpPost]
        public ApiResult Post(List<CookieModel> cookies)
        {
            //处理接收到Cookie后的动作
            return PublishCookie(cookies);
        }

        private ApiResult PublishCookie(List<CookieModel> cookies)
        {
            //_logger.LogInformation(JsonConvert.SerializeObject(cookies));
            string redisConnectionString;
            try
            {
                redisConnectionString = _configuration["RedisConnectionString"];
                if (string.IsNullOrEmpty(redisConnectionString))
                    return new ApiResult().Fail("未获取到RedisConnectionString");
                //_logger.LogInformation(redisConnectionString);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取配置异常：" + ex.Message);
                return new ApiResult().Fail("获取配置异常：" + ex.Message);
            }

            RedisHelper redisHelper;
            try
            {
                redisHelper = new RedisHelper(redisConnectionString);
                redisHelper.SelectDB();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "连接Redis异常：" + ex.Message);
                return new ApiResult().Fail("连接Redis异常：" + ex.Message);
            }

            var domains = cookies.Select(a => a.domain).Distinct().ToList();
            foreach (var domain in domains)
            {
                var cookiesByDomain = cookies.Where(a => a.domain == domain).ToList();
                redisHelper.SetString("COOKIE:" + domain, JsonConvert.SerializeObject(cookiesByDomain));
                redisHelper.Publish(new StackExchange.Redis.RedisChannel("COOKIE_" + domain.Replace(".", "_"), StackExchange.Redis.RedisChannel.PatternMode.Auto), new StackExchange.Redis.RedisValue(JsonConvert.SerializeObject(cookiesByDomain)));
            }
            return new ApiResult().Success();
        }
    }
}