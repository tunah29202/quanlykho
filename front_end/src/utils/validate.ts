import tl from "@/utils/locallize";

export default {
  required: (rule: any, value: any, callback: any) => {
    if (value === '' || value === null || value === undefined) {
      callback(new Error(tl('Common', 'ValidateFieldRequied', [rule.label ? rule.label : rule.field])))
    } else {
      callback()
    }
  },
  requiredRadioRule: (value: any, name: string) => {
    return value !== null || tl("Common", "ValidateFieldRequied", [name]);
  },
  minLengthRule: (value: any, name: string, min: number) => {
    return (
      value?.length >= min || tl("Common", "ValidateMinLength", [name, min])
    );
  },
  
  maxLengthRule: (rule: any, value: any, callback: any) => {
    if (value !== null && value.length > rule.max) {
      callback(new Error(tl('Common', 'ValidateMaxLength', [rule.label ? rule.label : rule.field, `${rule.max}`])))
    } else {
      callback()
    }
  },
  validatePositiveNumber: (rule: any, value: any, callback: any) => {
    if (value) {
      const numericValue = parseFloat(value)
      if (!isNaN(numericValue) && numericValue >= 0) {
        callback()
      } else {
        callback(new Error(tl('Common', 'ValidatePositiveNumber', [rule.label ? rule.label : rule.field])))
      }
    }
    callback()
  },
  lengthRule: (value: any, name: string, length: number) => {
    return (
      value?.length === length || tl("Common", "ValidateLength", [name, length])
    );
  },
  emailRule: (rule: any, value: any, callback: any) => {
    if (!value) callback()
  
    if (
      /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/.test(
        value
      )
    ) {
      callback()
    } else {
      callback(new Error(tl('Common', 'ValidateInvalid', [rule.label ? rule.label : rule.field])))
    }
  },
  dateRule: (value: any, name: string) => {
    return (
      /^([12]\d{3})[-](((0[13456789]|1[0-2])[-](0[1-9]|[12][0-9]|3[01]))|02[-](0[1-9]|[12][0-9]))$/.test(
        value
      ) || tl("Common", "ValidateInvalid", [name])
    );
  },
  phoneNumberRule: (rule: any, value: any, callback: any) => {
    if (!value) callback()
  
    if (/^[+0-9][./0-9]{8,19}$/.test(value)) {
      callback()
    } else {
      callback(new Error(tl('Common', 'ValidateInvalid', [rule.label ? rule.label : rule.field])))
    }
  },
  checkPassword: (value: any, newVal: any, name: string) => {
    return value === newVal || tl("User", "OtherPassWord", [name, length]);
  },
};
