<template>
  <a-drawer
    :visible="visible"
    :title=" isAdd ? '新增支付接口' : '修改支付接口'"
    :maskClosable="false"
    @close="onClose"
    :drawer-style="{ overflow: 'hidden' }"
    :body-style="{ paddingBottom: '80px', overflow: 'auto' }"
    width="40%">
    <a-form-model ref="infoFormModel" :model="saveObject" layout="vertical" :rules="rules">
      <a-row :gutter="16">
        <a-col :span="24">
          <p class="ag-drawer-title">基本配置</p>
        </a-col>
        <a-col :span="12">
          <a-form-model-item label="接口代码" prop="ifCode">
            <a-input v-model="saveObject.ifCode" placeholder="请输入" :disabled="!isAdd" />
          </a-form-model-item>
        </a-col>
        <a-col :span="12">
          <a-form-model-item label="接口名称" prop="ifName">
            <a-input v-model="saveObject.ifName" placeholder="请输入" />
          </a-form-model-item>
        </a-col>
        <a-col :span="12">
          <a-form-model-item label="是否支持普通商户模式" prop="isMchMode">
            <a-radio-group v-model="saveObject.isMchMode">
              <a-radio :value="1">
                支持
              </a-radio>
              <a-radio :value="0">
                不支持
              </a-radio>
            </a-radio-group>
          </a-form-model-item>
        </a-col>
        <a-col :span="12">
          <a-form-model-item label="是否支持服务商子商户模式" prop="isIsvMode">
            <a-radio-group v-model="saveObject.isIsvMode">
              <a-radio :value="1">
                支持
              </a-radio>
              <a-radio :value="0">
                不支持
              </a-radio>
            </a-radio-group>
          </a-form-model-item>
        </a-col>
        <a-col :span="24">
          <a-form-model-item label="支付参数配置页面类型" prop="configPageType">
            <a-radio-group v-model="saveObject.configPageType">
              <a-radio :value="1">
                根据接口配置定义描述渲染页面
              </a-radio>
              <a-radio :value="2">
                自定义页面
              </a-radio>
            </a-radio-group>
          </a-form-model-item>
        </a-col>
        <a-col :span="12">
          <a-form-model-item label="是否支持进件" prop="isSupportApplyment">
            <a-radio-group v-model="saveObject.isSupportApplyment">
              <a-radio :value="1">
                支持
              </a-radio>
              <a-radio :value="0">
                不支持
              </a-radio>
            </a-radio-group>
          </a-form-model-item>
        </a-col>
        <a-col :span="12">
          <a-form-model-item label="是否开启进件" prop="isOpenApplyment">
            <a-radio-group v-model="saveObject.isOpenApplyment" :disabled="!saveObject.isSupportApplyment">
              <a-radio :value="1">
                开启
              </a-radio>
              <a-radio :value="0">
                关闭
              </a-radio>
            </a-radio-group>
          </a-form-model-item>
        </a-col>
        <a-col :span="12">
          <a-form-model-item label="是否支持对账" prop="isSupportCheckBill">
            <a-radio-group v-model="saveObject.isSupportCheckBill">
              <a-radio :value="1">
                支持
              </a-radio>
              <a-radio :value="0">
                不支持
              </a-radio>
            </a-radio-group>
          </a-form-model-item>
        </a-col>
        <a-col :span="12">
          <a-form-model-item label="是否开启对账" prop="isOpenCheckBill">
            <a-radio-group v-model="saveObject.isOpenCheckBill" :disabled="!saveObject.isSupportCheckBill">
              <a-radio :value="1">
                开启
              </a-radio>
              <a-radio :value="0">
                关闭
              </a-radio>
            </a-radio-group>
          </a-form-model-item>
        </a-col>
        <a-col :span="12">
          <a-form-model-item label="是否支持提现" prop="isSupportCashout">
            <a-radio-group v-model="saveObject.isSupportCashout">
              <a-radio :value="1">
                支持
              </a-radio>
              <a-radio :value="0">
                不支持
              </a-radio>
            </a-radio-group>
          </a-form-model-item>
        </a-col>
        <a-col :span="12">
          <a-form-model-item label="是否开启提现" prop="isOpenCashout">
            <a-radio-group v-model="saveObject.isOpenCashout" :disabled="!saveObject.isSupportCashout">
              <a-radio :value="1">
                开启
              </a-radio>
              <a-radio :value="0">
                关闭
              </a-radio>
            </a-radio-group>
          </a-form-model-item>
        </a-col>
        <a-col :span="24" v-if="saveObject.isIsvMode == 1 && this.saveObject.configPageType === 1">
          <a-form-model-item label="服务商接口配置定义描述" prop="isvParams">
            <a-input v-model="saveObject.isvParams" placeholder="请输入" type="textarea" />
          </a-form-model-item>
        </a-col>
        <a-col :span="24" v-if="saveObject.isIsvMode == 1 && this.saveObject.configPageType === 1">
          <a-form-model-item label="特约商户接口配置定义描述" prop="isvsubMchParams">
            <a-input v-model="saveObject.isvsubMchParams" placeholder="请输入" type="textarea" />
          </a-form-model-item>
        </a-col>
        <a-col :span="24" v-if="saveObject.isMchMode == 1 && this.saveObject.configPageType === 1">
          <a-form-model-item label="普通商户接口配置定义描述" prop="normalMchParams">
            <a-input v-model="saveObject.normalMchParams" placeholder="请输入" type="textarea" />
          </a-form-model-item>
        </a-col>
        <a-col :span="12">
          <a-form-model-item label="状态" prop="state">
            <a-radio-group v-model="saveObject.state">
              <a-radio :value="1">
                启用
              </a-radio>
              <a-radio :value="0">
                停用
              </a-radio>
            </a-radio-group>
          </a-form-model-item>
        </a-col>
        <a-col :span="12">
          <a-form-model-item label="备注" prop="remark">
            <a-input v-model="saveObject.remark" placeholder="请输入" />
          </a-form-model-item>
        </a-col>
      </a-row>
      <a-row justify="space-between" type="flex">
        <a-col :span="24">
          <a-divider orientation="left"/>
        </a-col>
        <a-col :span="24">
          <p class="ag-drawer-title">支付方式</p>
        </a-col>
      </a-row>
      <a-row :gutter="16">
        <a-col :span="24">
          <a-form-model-item label="支持的支付方式" prop="checkedList">
            <!--<a-checkbox-group v-model="checkedList" :options="wayCodesOptions" @change="onWayCodesChange" />-->
            <a-checkbox-group v-model="checkedList">
              <a-row v-for="(group, index) in groupedWays" :key="index">
                <h3>{{ group.name }}</h3>
                <a-col :span="6" v-for="(way, i) in group.ways" :key="i">
                  <a-checkbox :value="way.wayCode">{{ way.wayName }}</a-checkbox>
                </a-col>
              </a-row>
            </a-checkbox-group>
          </a-form-model-item>
        </a-col>
      </a-row>
      <a-row justify="space-between" type="flex">
        <a-col :span="24">
          <a-divider orientation="left"/>
        </a-col>
        <a-col :span="24">
          <p class="ag-drawer-title">页面展示</p>
        </a-col>
      </a-row>
      <a-row :gutter="16">
        <a-col :span="12">
          <a-form-model-item label="卡片icon" prop="icon">
            <AgUpload
              :action="action"
              accept=".jpg, .jpeg, .png"
              bind-name="icon"
              :urls="[saveObject.icon]"
              @uploadSuccess="uploadSuccess"
            >
              <template slot="uploadSlot" slot-scope="{loading}">
                <a-button class="ag-upload-btn"> <a-icon :type="loading ? 'loading' : 'upload'" /> 上传 </a-button>
              </template>
            </AgUpload>
          </a-form-model-item>
        </a-col>
        <a-col :span="12">
          <a-form-model-item label="卡片背景色" prop="bgColor">
            <!--<a-input style="height: 66px; margin-top: 8px;" v-model="saveObject.bgColor" placeholder="请输入" />-->
            <colorPicker v-model="saveObject.bgColor" style="height: 66px; margin-top: 8px;"/>
          </a-form-model-item>
        </a-col>
      </a-row>
    </a-form-model>

    <div class="drawer-btn-center">
      <a-button @click="onClose" icon="close" :style="{ marginRight: '8px' }">取消</a-button>
      <a-button type="primary" @click="onSubmit" icon="check" >保存</a-button>
    </div>

  </a-drawer>
</template>

<script>
import AgUpload from '@/components/AgUpload/AgUpload'
import { API_URL_IFDEFINES_LIST, API_URL_PAYWAYS_LIST, req, upload } from '@/api/manage'

export default {
  components: {
    AgUpload
  },
  props: {
    callbackFunc: { type: Function, default: () => () => ({}) }
  },

  data () {
    const validateNormalMchParams = (rule, value, callback) => { // 普通商户接口配置定义描述 验证器
      if (this.saveObject.isMchMode === 1 && this.saveObject.configPageType === 1 && !value) {
        callback(new Error('请输入普通商户接口配置定义描述'))
      }
      callback()
    }
    const validateIsvParams = (rule, value, callback) => { // 服务商接口配置定义描述 验证器
      if (this.saveObject.isIsvMode === 1 && this.saveObject.configPageType === 1 && !value) {
        callback(new Error('请输入服务商接口配置定义描述'))
      }
      callback()
    }
    const validateIsvsubMchParams = (rule, value, callback) => { // 特约商户接口配置定义描述 验证器
      if (this.saveObject.isIsvMode === 1 && this.saveObject.configPageType === 1 && !value) {
        callback(new Error('请输入特约商户接口配置定义描述'))
      }
      callback()
    }
    const validateWayCodes = (rule, value, callback) => { // 支付方式 验证器
      if (this.checkedList.length <= 0) {
        callback(new Error('请选择支付方式'))
      }
      callback()
    }
    return {
      isAdd: true, // 新增 or 修改
      visible: false, // 抽屉开关
      action: upload.ifBG, // 上传图标地址
      ifCode: '', // 支付接口定义id
      saveObject: {}, // 数据对象
      rules: {
        ifCode: [{ required: true, message: '请输入接口代码', trigger: 'blur' }],
        ifName: [{ required: true, message: '请输入接口名称', trigger: 'blur' }],
        normalMchParams: [{ validator: validateNormalMchParams, trigger: 'blur' }],
        isvParams: [{ validator: validateIsvParams, trigger: 'blur' }],
        isvsubMchParams: [{ validator: validateIsvsubMchParams, trigger: 'blur' }],
        checkedList: [{ required: true, validator: validateWayCodes, trigger: 'blur' }]
      },
      // wayCodesOptions: [], // 支付方式多选框选项列表
      groupedWays: [], // 支付方式多选框选项列表
      checkedList: [] // 选中的数据
    }
  },
  created () {
    this.payWayList()
  },
  methods: {
    // 抽屉显示
    show (ifCode) {
      this.isAdd = !ifCode
       // 数据清空
      this.saveObject = {
        isMchMode: 1,
        isIsvMode: 1,
        state: 1,
        configPageType: 1,
        isSupportApplyment: 0,
        isOpenApplyment: 0,
        isSupportCheckBill: 0,
        isOpenCheckBill: 0,
        isSupportCashout: 0,
        isOpenCashout: 0,
        bgColor: '#1a53ff'
      }

      if (this.$refs.infoFormModel !== undefined) {
        this.$refs.infoFormModel.resetFields()
      }

      const that = this
      if (!this.isAdd) { // 修改信息 延迟展示弹层
        that.ifCode = ifCode
        // 拉取详情
        req.getById(API_URL_IFDEFINES_LIST, ifCode).then(res => {
          that.saveObject = res
          const newItems = [] // 多选框赋值
          res.wayCodes.forEach(item => {
            newItems.push(item.wayCode)
          })
           that.checkedList = newItems
        })
        this.visible = true
      } else {
        this.checkedList = [] // 多选框设置空
        that.visible = true // 展示弹层信息
      }
    },
    onClose () {
      this.visible = false
    },
    // 表单提交
    onSubmit () {
      const that = this
      this.$refs.infoFormModel.validate(valid => {
        if (valid) { // 验证通过
          that.saveObject.wayCodeStrs = this.checkedList.join(',')
          // 请求接口
          if (that.isAdd) {
            req.add(API_URL_IFDEFINES_LIST, that.saveObject).then(res => {
              that.$message.success('新增成功')
              that.visible = false
              that.callbackFunc() // 刷新列表
            })
          } else {
            req.updateById(API_URL_IFDEFINES_LIST, that.ifCode, that.saveObject).then(res => {
              that.$message.success('修改成功')
              that.visible = false
              that.callbackFunc() // 刷新列表
            })
          }
        }
      })
    },
    groupBy (list, key) {
      return list.reduce((acc, item) => {
        (acc[item[key]] = acc[item[key]] || []).push(item)
        return acc
      }, {})
    },
    getGroupName (wayType) {
      switch (wayType) {
        case 'WECHAT':
          return '微信'
        case 'ALIPAY':
          return '支付宝'
        case 'YSFPAY':
          return '云闪付'
        case 'UNIONPAY':
          return '银联'
        case 'DCEPPAY':
          return '数字人民币'
        default:
          return '其他'
      }
    },
    // 支付方式列表
    payWayList () {
      const that = this
      req.list(API_URL_PAYWAYS_LIST, { 'pageSize': '-1' }).then(res => {
        const ways = res.records

        const groupedWays = that.groupBy(ways, 'wayType')

        // 指定顺序排序
        const order = ['WECHAT', 'ALIPAY', 'YSFPAY', 'UNIONPAY', 'DCEPPAY', 'OTHER']
        const sortedGroupedWays = Object.fromEntries(
            Object.entries(groupedWays)
                .sort(([keyA], [keyB]) => {
                  const indexA = order.indexOf(keyA)
                  const indexB = order.indexOf(keyB)
                  return indexA - indexB
                })
        )

        for (const wayType in sortedGroupedWays) {
          const group = {
            name: that.getGroupName(wayType),
            ways: []
          }
          for (const way of sortedGroupedWays[wayType]) {
            group.ways.push({ wayCode: way.wayCode, wayName: way.wayName })
          }
          that.groupedWays.push(group)
        }

        // res.records.forEach(item => {
        //   that.wayCodesOptions.push({
        //     label: item.wayName,
        //     value: item.wayCode
        //   })
        // })
      })
    },
    // 上传文件成功回调方法，参数fileList为已经上传的文件列表，name是自定义参数
    uploadSuccess (name, fileList) {
      const [firstItem] = fileList
      this.saveObject[name] = firstItem?.url
      this.$forceUpdate()
    },
    onWayCodesChange (checkedValues) {
      this.checkedList = checkedValues
    }
  }
}
</script>

<style lang="less" scoped>
  .ag-drawer-title {
    font-size: 16px;
    font-weight: 600;
    width: 100%;
    margin-bottom: 15px;
  }

  .ag-upload-btn {
    height: 66px;
    margin-top: 8px;
  }

  .m-colorPicker {
    height: 66px;
    width: 100%;
    margin-top: 8px;
    border: 1px solid #d9d9d9;
    border-radius: 4px;

    /deep/ .colorBtn {
      height: 48px;
      width: calc(100% - 16px);
      margin: 8px;
      border-radius: 4px;
    }

    /deep/ .box.open {
      z-index: 3;
    }

    /deep/ .bd h3:nth-of-type(3) {
      cursor: pointer;
    }
  }
</style>
