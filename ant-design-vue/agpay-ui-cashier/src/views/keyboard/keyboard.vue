<!--
 * @Author: your name
 * @Date: 2020-12-23 10:46:37
 * @LastEditors: your name
 * @LastEditTime: 2020-12-30 11:02:25
 * @FilePath: \pay\src\components\keyboard\keyboard.vue
 * @SendWord: 永无BUG vite⚡
-->

<template>
  <div class="keyboard">
    <div class="keyboard-top">
      <div class="triangle-topleft-k"></div>
      <div class="keyboard-tite">@Ag支付</div>
      <div class="triangle-topleft-k" @click="concealSateFn">
        <div
            class="triangle-topleft"
            :style="
            concealSate ? '' : 'transform:rotate(-135deg);margin-top: 12px;'
          "
        ></div>
      </div>
    </div>
    <div
        class="keyboard-main"
        v-show="concealSate"
        style="transition: all 1s ease"
    >
      <div
          v-for="(item, index) in numberList"
          :key="index"
          class="keyborad-key"
      >
        <!-- @click="onKeyboard(it, $event)" -->
        <div
            ref="number"
            class="number"
            v-for="(it, ind) in item"
            :key="ind"
            @touchstart.prevent="goTouchstart(it, $event)"
            @touchend.prevent="goTouchend(it, $event)"
        >
          {{ it != "del" ? it : "" }}
          <template class="" v-if="it == 'del'">
            <!-- <div class="jiao"></div>
            <div class="juxing"></div> -->
            <img src="../../assets/icon/del.svg" alt="" />
          </template>
        </div>
      </div>
      <div class="keyborad-key">
        <div
            class="number zero"
            @touchstart.prevent="goTouchstart('zero', $event)"
            @touchend.prevent="goTouchend('zero', $event)"
        >
          0
        </div>
        <div
            class="number"
            @touchstart.prevent="goTouchstart('dot', $event)"
            @touchend.prevent="goTouchend('dot', $event)"
        >
          <div class="dot"></div>
        </div>
      </div>
    </div>

    <div
        :class="paymentClassFn"
        :style="'background:' + typeColor + ';'"
        @touchstart.prevent="goTouchstart('pay', $event)"
        @touchend.prevent="goTouchend('pay', $event)"
    >
      <div class="pay">付款</div>
    </div>
  </div>
</template>

<script>
export default {
  name: "Keyboard",
  data() {
    return {
      timeOutEvent: 0, //记录触摸时长
      tiemIntervalEvent: 0,
      concealSateC: true,
      numberList: [
        [1, 2, 3, "del"],
        [4, 5, 6, "C"],
        [7, 8, 9],
      ],
    };
  },
  computed: {
    paymentClassFn() {
      // `this` 指向 vm 实例
      let className1 = this.concealSate ? "payment" : "paymentConceal";
      let className2 = this.money != -1 && this.money != "" ? "" : "opacityS";
      return className1 + " " + className2;
    },
  },
  props: {
    typeColor: {
      type: String,
      default: "#07c160",
    },
    touchTypeColor: {
      type: String,
      default: "rgba(7, 130, 65, 1)",
    },
    money: {
      // eslint-disable-next-line vue/require-prop-type-constructor
      type: String | Number,
      default: -1,
    },
    concealSate: {
      type: Boolean,
      default: true,
    },
  },
  //emits: ["payment","conceal","delTheAmount","enterTheAmount"],
  mounted() {
    this.concealSateC = this.concealSate;
  },
  methods: {
    concealSateFn() {
      //   this.concealSateC = !this.concealSateC;
      this.$emit("conceal");
    },
    onKeyboard(item, $event) {
      setTimeout(() => {
        if (item == "pay") {
          $event.style.background = this.typeColor;
        } else {
          $event.style.background = "#fff";
        }
      }, 100);
      // animation: heartBeat 0.2s;
      if (item == "del") {
        this.$emit("delTheAmount", item);
        return;
      }
      if (item == "C") {
        this.$emit("clearTheAmount", item);
        return;
      }
      if (item == "pay") {
        this.$emit("payment");
        return;
      }
      let obj = {
        zero: 0,
        dot: ".",
      };
      if (typeof item != "number") {
        item = obj[item];
      }
      this.$emit("enterTheAmount", item);
    },

    goTouchstart(it, $event) {
      // console.log("goTouchstart");
      if (
          $event.target.localName == "img" ||
          $event.target.className == "dot" ||
          $event.target.className == "pay"
      ) {
        $event = $event.target.parentNode;
      } else {
        $event = $event.target;
      }
      if (it == "pay") {
        if (this.money != -1 && this.money != "") {
          $event.style.background = this.touchTypeColor;
        } else {
          return;
        }
      } else {
        $event.style.background = "rgba(197, 197, 197, 0.7)";
      }
      let _this = this;
      clearTimeout(_this.timeOutEvent);
      _this.timeOutEvent = setTimeout(function () {
        _this.timeOutEvent = 0;
        if (it == "del") {
          clearInterval(_this.tiemIntervalEvent);
          _this.delLong(it);
          return;
        }
        // 处理长按事件...
      }, 600);
    },
    // 手如果在600毫秒内就释放，则取消长按事件
    goTouchend(it, $event) {
      // console.log("goTouchend");
      if (
          $event.target.localName == "img" ||
          $event.target.className == "dot" ||
          $event.target.className == "pay"
      ) {
        $event = $event.target.parentNode;
      } else {
        $event = $event.target;
      }
      if (it == "pay") {
        if (this.money != -1 && this.money != "") {
          $event.style.background = this.typeColor;
        } else {
          return;
        }
      } else {
        $event.style.background = "#fff";
      }
      let _this = this;
      // console.log(_this.timeOutEvent);
      clearTimeout(_this.timeOutEvent);
      clearInterval(_this.tiemIntervalEvent);
      if (_this.timeOutEvent !== 0) {
        // 处理单击事件
        this.onKeyboard(it, $event);
      }
    },
    // 长按退格
    delLong(item) {
      // 定时触发
      this.tiemIntervalEvent = setInterval(() => {
        this.$emit("delTheAmount", item);
      }, 200);
    },
  },
};
</script>
<style lang="css" scoped>
@keyframes switchColor {
  0% {
    background-color: #fff;
  }
  50% {
    background-color: #bbbbbb;
  }
  100% {
    background-color: #fff;
  }
}

@font-face {
  font-family: "wxFirstFont";
  src: url("../../assets/wx-zt/WeChatSansSS-Bold.ttf"); /* IE9 */
}

img {
  margin: 0;
  padding: 0;
}
.keyboard {
  position: relative;
  background-color: #f5f7fa;
}
.keyboard-top {
  display: flex;
  justify-content: space-between;
  align-items: center;
  width: 100%;
  height: 60px;
  background: #fff;
  /*border: 1px solid #e1e2e6;*/
  /*border-bottom: 0px solid #e1e2e6;*/
  border-radius: 10px;
}
.keyboard-tite {
  /* margin-right: -40px; */
  font-size: 22px;
  font-family: wxFirstFont SC, PingFang SC, PingFang SC-Medium;
  font-weight: 500;
  text-align: center;
  color: #bbbdbf;
  letter-spacing: 1px;
}

.triangle-topleft-k {
  width: 200px;
  /* height: 100%; */
  display: flex;
  justify-content: flex-end;
}
.triangle-topleft {
  margin-top: -15px;
  margin-right: 30px;
  transform: rotate(45deg);
  width: 0;
  height: 0;
  border-bottom: 18px solid #b2b2b2;
  border-left: 18px solid transparent;
}
.keyboard-main {
  width: 100%;
}
.keyborad-key {
  display: flex;
  flex-flow: wrap;
}
/* .del {
  display: flex;
  flex-flow: nowrap;
} */
.jiao {
  margin-top: 4px;
  margin-right: -16px;
  width: 28px;
  height: 28px;
  transform: rotate(-45deg);
  border-top: 3px solid #2c2c2c;
  border-left: 3px solid #2c2c2c;
  border-radius: 4px;
}
.juxing {
  width: 40px;
  height: 37px;
  border: 3px solid #2c2c2c;
  border-left: none;
  border-radius: 4px;
}
.keyboard-main .number {
  /* flex-grow: 1; */
  display: flex;
  justify-content: center;
  align-items: center;
  /* width: 187px; */
  width: 24%;
  height: 155px;
  background: #fff;
  border: 1px solid #e1e2e6;
  border-bottom: none;
  border-left: none;
  font-size: 56px;
  font-family: PingFang SC, PingFang SC-Medium;
  font-weight: 500;
  text-align: center;
  line-height: 100px;
  color: #242526;
  letter-spacing: 2px;
  margin: 0.5%;
  border-radius: 10px;
}
.number img {
  height: 67px;
  width: 81px;
}
/* .number:active {

  background-color: rgba(197, 197, 197, 0.527);
} */
.dot {
  width: 6px;
  height: 6px;
  border-radius: 3px;
  background-color: #242526 !important;
}
.keyboard-main .zero {
  /* flex-grow: 2; */
  width: 49%;
  /* width: 374px; */
  margin: 0.5%;
}
.payment {
  flex-grow: 1;
  position: absolute;
  display: flex;
  align-items: center;
  bottom: 0;
  right: 0;
  width: calc(96vw / 4);
  height: 316px;
  background: #07c160;
  transition: all 0.3s ease;
  margin: 0.5%;
  border-radius: 10px;
}
.paymentConceal {
  flex-grow: 1;
  position: absolute;
  display: flex;
  align-items: center;
  right: 30px;
  top: -180px;
  /* width: 189px; */
  width: calc(100vw / 4);
  height: 150px;
  background: #07c160;
  border-radius: 10px;
  transition: all 0.3s ease;
}
.payment div {
  width: 100%;
  font-size: 36px;
  font-family: PingFang SC, PingFang SC-Medium;
  font-weight: 500;
  text-align: center;
  color: #ffffff;
  letter-spacing: 1px;
}
.paymentConceal div {
  width: 100%;
  font-size: 36px;
  font-family: PingFang SC, PingFang SC-Medium;
  font-weight: 500;
  text-align: center;
  color: #ffffff;
  letter-spacing: 1px;
}
.opacityS {
  opacity: 0.6;
}
</style>
